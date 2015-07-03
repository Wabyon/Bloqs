using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Bloqs.Data.Commands;
using Bloqs.Filters;
using Bloqs.Models;

namespace Bloqs.Controllers
{
    [AccessLogFilter]
    [TraceLogFilter]
    [RoutePrefix("{accountName}")]
    public class ContainerController : ApiController
    {
        private readonly AccountDbCommand _accountDbCommand =
            new AccountDbCommand(GlobalSettings.DefaultConnectionString);

        private readonly ContainerDbCommand _containerDbCommand =
            new ContainerDbCommand(GlobalSettings.DefaultConnectionString);

        [Route("api/{name}/attributes")]
        [HttpGet]
        public async Task<ContainerResponseModel> GetReferenceAsync([FromUri]string accountName, [FromUri]string name)
        {
            var account = await CheckAccessKey(accountName);
            var container = await _containerDbCommand.FindAsync(account, name);
            var response = container != null
                ? Mapper.Map<ContainerResponseModel>(container)
                : new ContainerResponseModel
                {
                    Name = name,
                };
            return response;
        }

        [Route("api/containers/list")]
        public async Task<IEnumerable<ContainerResponseModel>> GetListAsync([FromUri] string accountName)
        {
            var account = await CheckAccessKey(accountName);
            var count = await _containerDbCommand.CountAsync(account);
            var containers = await _containerDbCommand.GetListAsync(account,0,count);
            return Mapper.Map<IEnumerable<ContainerResponseModel>>(containers);
        }

        [Route("api/{name}/create")]
        [HttpPost]
        public async Task CreateIfNotExistsAsync([FromUri]string accountName, [FromUri]string name, [FromBody]ContainerRequestModel model)
        {
            var account = await CheckAccessKey(accountName);
            var container = await _containerDbCommand.FindAsync(account, name) ?? account.CreateContainer(name);
            container.LastModifiedUtcDateTime = DateTime.UtcNow;
            Mapper.Map(model, container);
            
            await _containerDbCommand.SaveAsync(container);
        }

        private async Task<Account> CheckAccessKey(string accountName)
        {
            var account = await _accountDbCommand.FindByNameAsync(accountName);
            if (account == null) throw new HttpResponseException(HttpStatusCode.NotFound);

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

            return account;
        }
    }
}