using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using Newtonsoft.Json;
using Bloqs.Data;

namespace Bloqs.Controllers
{
    [AllowAnonymous]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class BlobController : ApiController
    {
        private readonly ContainerRepository _conteinerRepository = new ContainerRepository(ConfigurationManager.ConnectionStrings["Default"].ConnectionString);
        private readonly BlobRepository _blobRepository = new BlobRepository(ConfigurationManager.ConnectionStrings["Default"].ConnectionString);

        [Route("blob/{containerName}/{name}")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetImage([FromUri] string containerName, [FromUri] string name)
        {
            var container = await CheckAccessKeyAndGetContainer(containerName, true);

            var blob = await _blobRepository.GetAsync(container.Id, name);

            if (blob == null) throw new HttpResponseException(HttpStatusCode.NoContent);

            var res = Request.CreateResponse(HttpStatusCode.OK);

            var content = new ByteArrayContent(blob.Image);

            content.Headers.ContentType = new MediaTypeHeaderValue(blob.Properties.ContentType);
            content.Headers.ContentDisposition = new ContentDispositionHeaderValue(blob.Properties.ContentDisposition ?? "attachment")
            {
                ModificationDate = blob.Properties.LastModifiedUtcDateTime,
                FileName = blob.Name,
                Size = blob.Properties.Length,
            };
            res.Content = content;
            return res;
        }

        [Route("blob/{containerName}/{name}/attribute")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetAttribute([FromUri] string containerName, [FromUri] string name)
        {
            var container = await CheckAccessKeyAndGetContainer(containerName, true);

            var blob = await _blobRepository.GetAttributeAsync(container.Id, name) ??
                       new BlobAttribute(new BlobProperties
                       {
                           ContentType = MimeMapping.GetMimeMapping(name),
                           ContentDisposition = "attachment",
                       })
                       {
                           Name = name,
                       };

            var res = Request.CreateResponse(HttpStatusCode.OK);
            var json = JsonConvert.SerializeObject(blob);
            var content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            res.Content = content;

            return res;
        }

        [Route("blob/{containerName}/save")]
        [HttpPost]
        public async Task<HttpResponseMessage> Save([FromUri] string containerName)
        {
            if (Request.Content.IsMimeMultipartContent() == false) throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

            var container = await CheckAccessKeyAndGetContainer(containerName, true);

            var provider = await Request.Content.ReadAsMultipartAsync();

            var attributeContent = provider.Contents.FirstOrDefault(x => x.Headers.ContentDisposition.Name == "attribute");

            if (attributeContent == null) throw new HttpResponseException(HttpStatusCode.BadRequest);

            var json = await attributeContent.ReadAsStringAsync();
            var blob = JsonConvert.DeserializeObject<Blob>(json);
            blob.Properties.LastModifiedUtcDateTime = DateTime.UtcNow;
            blob.Properties.ETag = CreateETag(blob.Properties.LastModifiedUtcDateTime.ToString("O"));

            var fileContent = provider.Contents.FirstOrDefault(x => x.Headers.ContentDisposition.Name == "file");

            if (fileContent == null) throw new HttpResponseException(HttpStatusCode.BadRequest);

            var file = await fileContent.ReadAsByteArrayAsync();

            blob.Image = file;
            blob.Properties.Length = file.LongLength;

            await _blobRepository.SaveAsync(container.Id, blob);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        private async Task<Container> CheckAccessKeyAndGetContainer(string containerName, bool throughCheckAccessKeyIfPublic = false)
        {
            var container = await _conteinerRepository.FindByNameAsync(containerName);
            if (container == null) throw new HttpResponseException(HttpStatusCode.NotFound);

            if (throughCheckAccessKeyIfPublic && container.IsPublic) return container;

            IEnumerable<string> s;

            if (!Request.Headers.TryGetValues("X-Bloqs-API-Key", out s))
                throw new HttpResponseException(HttpStatusCode.Unauthorized);

            var keys = s as string[] ?? s.ToArray();

            if (!keys.Any()) throw new HttpResponseException(HttpStatusCode.Unauthorized);

            var key = keys.FirstOrDefault();

            if (container.PrimaryAccessKey != key && container.SecondaryAccessKey != key)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

            return container;
        }

        private static string CreateETag(string s)
        {
            var provider = new SHA256CryptoServiceProvider();
            var hash = provider.ComputeHash(Encoding.UTF8.GetBytes(s + Guid.NewGuid()));
            var num = BitConverter.ToInt64(hash, 0);

            return string.Format("0x{0}", num.ToString("X"));
        }
    }
}
