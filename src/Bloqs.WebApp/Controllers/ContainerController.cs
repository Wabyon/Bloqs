using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using Bloqs.Data.Commands;
using Bloqs.Filters;
using Bloqs.Models;

namespace Bloqs.Controllers
{
    [RoutePrefix("accounts/{accountName}/containers")]
    [Authorize]
    public class ContainerController : Controller
    {
        private readonly AccountDbCommand _accountDbCommand =
            new AccountDbCommand(ConfigurationManager.ConnectionStrings["Default"].ConnectionString);

        private readonly ContainerDbCommand _containerDbCommand =
            new ContainerDbCommand(ConfigurationManager.ConnectionStrings["Default"].ConnectionString);

        [NoCache]
        [Route("")]
        [HttpGet]
        public async Task<ActionResult> Index(string accountName)
        {
            var account = await _accountDbCommand.FindByNameAsync(accountName);
            if (account == null) return HttpNotFound();
            if (account.Owner != User.Identity.Name) return new HttpUnauthorizedResult();

            var count = await _containerDbCommand.CountAsync(account);
            var containers = await _containerDbCommand.GetListAsync(account, 0, 10);

            ViewBag.AccountName = account.Name;
            ViewBag.Skip = 0;
            ViewBag.Take = 10;
            ViewBag.TotalCount = count;
            ViewBag.HasNext = !(count <= 10);
            ViewBag.HasPreview = false;

            return View(Mapper.Map<IEnumerable<ContainerIndexModel>>(containers));
        }

        [NoCache]
        [Route("list")]
        [HttpGet]
        public async Task<ActionResult> List(string accountName, int skip, int take)
        {
            var account = await _accountDbCommand.FindByNameAsync(accountName);
            if (account == null) return HttpNotFound();
            if (account.Owner != User.Identity.Name) return new HttpUnauthorizedResult();

            var count = await _containerDbCommand.CountAsync(account);
            var containers = await _containerDbCommand.GetListAsync(account, skip, take);

            ViewBag.Skip = skip;
            ViewBag.Take = take;
            ViewBag.TotalCount = count;
            ViewBag.HasNext = !(count <= skip + take);
            ViewBag.HasPreview = (skip != 0);

            return PartialView("_List", Mapper.Map<IEnumerable<ContainerIndexModel>>(containers));
        }

        [NoCache]
        [Route("new")]
        [HttpGet]
        public async Task<ActionResult> Create(string accountName)
        {
            var account = await _accountDbCommand.FindByNameAsync(accountName);
            if (account == null) return HttpNotFound();
            if (account.Owner != User.Identity.Name) return new HttpUnauthorizedResult();

            var container = account.CreateContainer("");

            return PartialView("_Create", Mapper.Map<ContainerCreateModel>(container));
        }

        [NoCache]
        [Route("new")]
        [HttpPost]
        public async Task<ActionResult> Create(string accountName, ContainerCreateModel model)
        {
            var account = await _accountDbCommand.FindByNameAsync(accountName);
            if (account == null) return HttpNotFound();
            if (account.Owner != User.Identity.Name) return new HttpUnauthorizedResult();

            if (!ModelState.IsValid) return PartialView("_Create", model);

            if (await _containerDbCommand.ExistsAsync(account, model.Name))
            {
                ModelState.AddModelError("Name", "この名前はすでに使用されています。");
                return PartialView("_Create", model);
            }

            var container = account.CreateContainer(model.Name);
            Mapper.Map(model, container);

            await _containerDbCommand.SaveAsync(container);

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [NoCache]
        [Route("{id}/detail")]
        [HttpGet]
        public async Task<ActionResult> Detail(string accountName, string id)
        {
            var account = await _accountDbCommand.FindByNameAsync(accountName);
            if (account == null) return HttpNotFound();
            if (account.Owner != User.Identity.Name) return new HttpUnauthorizedResult();

            var container = await _containerDbCommand.FindAsync(id);
            if (container == null) return HttpNotFound();
            return PartialView("_Detail", Mapper.Map<ContainerIndexModel>(container));
        }

        [NoCache]
        [Route("{id}/edit")]
        [HttpGet]
        public async Task<ActionResult> Edit(string accountName, string id)
        {
            var account = await _accountDbCommand.FindByNameAsync(accountName);
            if (account == null) return HttpNotFound();
            if (account.Owner != User.Identity.Name) return new HttpUnauthorizedResult();

            var container = await _containerDbCommand.FindAsync(id);
            if (container == null) return HttpNotFound();
            return PartialView("_Edit", Mapper.Map<ContainerEditModel>(container));
        }

        [NoCache]
        [Route("{id}/edit")]
        [HttpPost]
        public async Task<ActionResult> Edit(string accountName, string id, ContainerEditModel model)
        {
            var account = await _accountDbCommand.FindByNameAsync(accountName);
            if (account == null) return HttpNotFound();
            if (account.Owner != User.Identity.Name) return new HttpUnauthorizedResult();

            if (!ModelState.IsValid) return PartialView("_Edit", model);

            var container = await _containerDbCommand.FindAsync(id);
            if (container == null) return HttpNotFound();

            Mapper.Map(model, container);
            container.LastModifiedUtcDateTime = DateTime.UtcNow;

            await _containerDbCommand.SaveAsync(container);

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [NoCache]
        [Route("{id}/delete")]
        [HttpGet]
        public async Task<ActionResult> Delete(string accountName, string id)
        {
            var account = await _accountDbCommand.FindByNameAsync(accountName);
            if (account == null) return HttpNotFound();
            if (account.Owner != User.Identity.Name) return new HttpUnauthorizedResult();

            var container = await _containerDbCommand.FindAsync(id);
            if (container == null) return HttpNotFound();

            return PartialView("_Delete", Mapper.Map<ContainerDeleteModel>(container));
        }

        [NoCache]
        [Route("{id}/delete")]
        [HttpPost]
        public async Task<ActionResult> Delete(string accountName, string id, ContainerDeleteModel model)
        {
            var account = await _accountDbCommand.FindByNameAsync(accountName);
            if (account == null) return HttpNotFound();
            if (account.Owner != User.Identity.Name) return new HttpUnauthorizedResult();

            if (!ModelState.IsValid) return PartialView("_Delete", model);

            var container = await _containerDbCommand.FindAsync(id);
            if (container == null) return HttpNotFound();

            await _containerDbCommand.DeleteAsync(id);

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}