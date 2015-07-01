using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using AutoMapper;
using Bloqs.Data.Commands;
using Bloqs.Models;

namespace Bloqs.Controllers
{
    [RoutePrefix("{accountName}")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class BlobController : ApiController
    {
        private readonly AccountDbCommand _accountDbCommand =
            new AccountDbCommand(ConfigurationManager.ConnectionStrings["Default"].ConnectionString);

        private readonly ContainerDbCommand _containerDbCommand =
            new ContainerDbCommand(ConfigurationManager.ConnectionStrings["Default"].ConnectionString);

        private readonly BlobDbCommand _blobDbCommand =
            new BlobDbCommand(ConfigurationManager.ConnectionStrings["Default"].ConnectionString);

        [Route("{containerName}/{name}")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetImageAsync([FromUri]string accountName, [FromUri] string containerName, [FromUri] string name)
        {
            var container = await CheckAccessKey(accountName, containerName, true);
            var blob = await _blobDbCommand.FindAsync(container, name);
            if (blob == null) throw new HttpResponseException(HttpStatusCode.NoContent);
            var res = Request.CreateResponse(HttpStatusCode.OK);
            var content = new ByteArrayContent(blob.Image);
            content.Headers.ContentType = new MediaTypeHeaderValue(blob.Properties.ContentType ?? "application/octet-stream");
            content.Headers.ContentDisposition = new ContentDispositionHeaderValue(blob.Properties.ContentDisposition ?? "attachment")
            {
                ModificationDate = blob.Properties.LastModifiedUtcDateTime,
                FileName = blob.Name,
                Size = blob.Properties.Length,
            };
            res.Content = content;
            return res;
        }

        [Route("api/{containerName}/{name}/attributes")]
        [HttpGet]
        public async Task<BlobResponseModel> GetReferenceAsync([FromUri]string accountName, [FromUri] string containerName, [FromUri] string name)
        {
            var container = await CheckAccessKey(accountName, containerName);
            var blob = await _blobDbCommand.GetAttributesAsync(container, name);
            
            if (blob != null) return Mapper.Map<BlobResponseModel>(blob);

            blob = container.CreateBlobAttributes(name);
            blob.Properties.ContentType = MimeMapping.GetMimeMapping(name);
            blob.Properties.ContentDisposition = "attachment";

            return Mapper.Map<BlobResponseModel>(blob);
        }

        [Route("api/{containerName}/blobs/list")]
        [HttpGet]
        public async Task<IEnumerable<BlobResponseModel>> GetListAsync([FromUri] string accountName,
            [FromUri] string containerName)
        {
            var container = await CheckAccessKey(accountName, containerName);
            var count = await _blobDbCommand.CountAsync(container);
            var blobs = await _blobDbCommand.GetAttributesListAsync(container, 0, count);
            return Mapper.Map<IEnumerable<BlobResponseModel>>(blobs);
        }

        [Route("api/{containerName}/save")]
        [HttpPost]
        public async Task<HttpResponseMessage> SaveAsync([FromUri]string accountName, [FromUri] string containerName)
        {
            if (Request.Content.IsMimeMultipartContent() == false) throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            var container = await CheckAccessKey(accountName, containerName);

            var provider = await Request.Content.ReadAsMultipartAsync();
            var attributeContent = provider.Contents.FirstOrDefault(x => x.Headers.ContentDisposition.Name == "attributes");
            if (attributeContent == null) throw new HttpResponseException(HttpStatusCode.BadRequest);
            var blobRequest = await attributeContent.ReadAsAsync<BlobRequestModel>();
            blobRequest.Properties.LastModifiedUtcDateTime = DateTime.UtcNow;
            var blobAttributes = container.CreateBlobAttributes(blobRequest.Name);
            Mapper.Map(blobRequest, blobAttributes);

            var fileContent = provider.Contents.FirstOrDefault(x => x.Headers.ContentDisposition.Name == "file");
            if (fileContent == null) throw new HttpResponseException(HttpStatusCode.BadRequest);

            var file = await fileContent.ReadAsByteArrayAsync();

            var blob = new Blob(blobAttributes)
            {
                Image = file,
                Properties = {LastModifiedUtcDateTime = DateTime.UtcNow},
            };

            await _blobDbCommand.SaveAsync(blob);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        private async Task<Container> CheckAccessKey(string accountName, string containerName, bool throughCheckAccessKeyIfPublic = false)
        {
            var account = await _accountDbCommand.FindByNameAsync(accountName);
            if (account == null) throw new HttpResponseException(HttpStatusCode.NotFound);

            var container = await _containerDbCommand.FindAsync(account, containerName);
            if (container == null) throw new HttpResponseException(HttpStatusCode.NotFound);

            if (throughCheckAccessKeyIfPublic && container.IsPublic) return container;

            IEnumerable<string> s;

            if (!Request.Headers.TryGetValues("X-Bloqs-API-Key", out s))
                throw new HttpResponseException(HttpStatusCode.Unauthorized);

            var keys = s as string[] ?? s.ToArray();

            if (!keys.Any()) throw new HttpResponseException(HttpStatusCode.Unauthorized);

            var key = keys.FirstOrDefault();

            if (account.PrimaryAccessKey != key && account.SecondaryAccessKey != key)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

            return container;
        }
    }
}
