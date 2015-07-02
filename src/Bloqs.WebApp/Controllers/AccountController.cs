using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using Bloqs.Data.Commands;
using Bloqs.Filters;
using Bloqs.Models;

namespace Bloqs.Controllers
{
    [TraceLogFilter]
    [AccessLogFilter]
    [RoutePrefix("accounts")]
    [Authorize]
    public class AccountController : Controller
    {
        private readonly AccountDbCommand _accountDbCommand =
            new AccountDbCommand(GlobalSettings.DefaultConnectionString);
        private readonly StorageDbCommand _storageDbCommand =
            new StorageDbCommand(GlobalSettings.DefaultConnectionString);

        [NoCache]
        [Route("")]
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var count = await _accountDbCommand.CountByOwnerAsync(User.Identity.Name);
            var accounts = await _accountDbCommand.GetListByOwnerAsync(User.Identity.Name, 0, 10);

            ViewBag.Skip = 0;
            ViewBag.Take = 10;
            ViewBag.TotalCount = count;
            ViewBag.HasNext = !(count <= 10);
            ViewBag.HasPreview = false;

            return View(Mapper.Map<IEnumerable<AccountIndexModel>>(accounts));
        }

        [TraceLogFilter]
        [AccessLogFilter]
        [NoCache]
        [Route("list")]
        [HttpGet]
        public async Task<ActionResult> List(int skip, int take)
        {
            var count = await _accountDbCommand.CountByOwnerAsync(User.Identity.Name);
            var accounts = await _accountDbCommand.GetListByOwnerAsync(User.Identity.Name, skip, take);
            
            ViewBag.Skip = skip;
            ViewBag.Take = take;
            ViewBag.TotalCount = count;
            ViewBag.HasNext = !(count <= skip + take);
            ViewBag.HasPreview = (skip != 0);

            return PartialView("_List",Mapper.Map<IEnumerable<AccountIndexModel>>(accounts));
        }

        [NoCache]
        [Route("{id}/detail")]
        [HttpGet]
        public async Task<ActionResult> Detail(string id)
        {
            var account = await _accountDbCommand.FindAsync(id);
            if (account == null) return HttpNotFound();
            if (account.Owner != User.Identity.Name) return new HttpUnauthorizedResult();

            var model = Mapper.Map<AccountIndexModel>(account);

            return PartialView("_Detail", model);
        }

        [NoCache]
        [Route("new")]
        [HttpGet]
        public async Task<ActionResult> Create()
        {
            var model = new AccountCreateModel{ UsePersonalStorage = true };
            var personalStorages = (await _storageDbCommand.GetListByOwnerAsync(User.Identity.Name, 0, int.MaxValue)).Where(x => x.StorageType == StorageType.Personal).ToArray();
            model.PersonalSorages = personalStorages.Select(x => new SelectListItem{Text = x.Name, Value = x.Id}).ToList();
            return View("Create", model);
        }

        [NoCache]
        [Route("new")]
        [HttpPost]
        public async Task<ActionResult> Create(AccountCreateModel model)
        {
            var personalStorages = (await _storageDbCommand.GetListByOwnerAsync(User.Identity.Name, 0, int.MaxValue)).Where(x => x.StorageType == StorageType.Personal).ToArray();

            if (!ModelState.IsValid)
            {
                model.PersonalSorages = personalStorages.Select(x => new SelectListItem { Text = x.Name, Value = x.Id }).ToList();
                return View("Create", model);
            }

            if (Account.IsReservedName(model.Name))
            {
                ModelState.AddModelError("Name", "この名前は使用できません。");
                model.PersonalSorages = personalStorages.Select(x => new SelectListItem { Text = x.Name, Value = x.Id }).ToList();
                return View("Create", model);
            }

            if (await _accountDbCommand.ExistsAsync(model.Name))
            {
                ModelState.AddModelError("Name", "この名前はすでに使用されています。");
                model.PersonalSorages = personalStorages.Select(x => new SelectListItem { Text = x.Name, Value = x.Id }).ToList();
                return View("Create", model);
            }

            if (model.UsePersonalStorage && string.IsNullOrWhiteSpace(model.PersonalStorageId))
            {
                ModelState.AddModelError("PersonalStorageId", @"個別ストレージを使用する場合は、ストレージを選択する必要があります。
選択リストに出てこない場合は、ストレージタブから新しい個別ストレージを登録してください。");
                model.PersonalSorages = personalStorages.Select(x => new SelectListItem { Text = x.Name, Value = x.Id }).ToList();
                return View("Create", model);
            }

            Account account;
            if (model.UsePersonalStorage)
            {
                var storage = personalStorages.FirstOrDefault(x => x.Id == model.PersonalStorageId);
                if (storage == null)
                {
                    ModelState.AddModelError("PersonalStorageId", @"ストレージの選択が不正なようです。もう一度ストレージを選び直してください。");
                    model.PersonalSorages = personalStorages.Select(x => new SelectListItem { Text = x.Name, Value = x.Id }).ToList();
                    return View("Create", model);
                }

                account = Account.NewAccount(model.Name, User.Identity.Name, StorageType.Personal);
                account.Storages.Add(storage);
                Mapper.Map(model, account);
            }
            else
            {
                account = Account.NewAccount(model.Name, User.Identity.Name, StorageType.Common);
                Mapper.Map(model, account);
            }

            await _accountDbCommand.CreateAsync(account);

            return RedirectToAction("Index");
        }

        [NoCache]
        [Route("{id}/edit")]
        [HttpGet]
        public async Task<ActionResult> Edit(string id)
        {
            var account = await _accountDbCommand.FindAsync(id);
            if (account == null) return HttpNotFound();
            if (account.Owner != User.Identity.Name) return new HttpUnauthorizedResult();

            var model = Mapper.Map<AccountEditModel>(account);

            return PartialView("_Edit", model);
        }

        [NoCache]
        [Route("{id}/edit")]
        [HttpPost]
        public async Task<ActionResult> Edit(string id, AccountEditModel model)
        {
            if (!ModelState.IsValid) return PartialView("_Edit", model);
            if (id != model.Id) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var account = await _accountDbCommand.FindAsync(model.Id);
            if (account == null) return HttpNotFound();
            if (account.Owner != User.Identity.Name) return new HttpUnauthorizedResult();

            Mapper.Map(model, account);

            account.LastModifiedUtcDateTime = DateTime.UtcNow;

            await _accountDbCommand.UpdateAsync(account.Id, account);

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [NoCache]
        [Route("key-generate")]
        [HttpGet]
        public string GenerateKey()
        {
            var key = Account.CreateNewAccessKeyString();
            return key;
        }

        [NoCache]
        [Route("{id}/delete")]
        [HttpGet]
        public async Task<ActionResult> Delete(string id)
        {
            var account = await _accountDbCommand.FindAsync(id);
            if (account == null) return HttpNotFound();
            if (account.Owner != User.Identity.Name) return new HttpUnauthorizedResult();

            var model = Mapper.Map<AccountDeleteModel>(account);

            return PartialView("_Delete", model);
        }

        [NoCache]
        [Route("{id}/delete")]
        [HttpPost]
        public async Task<ActionResult> Delete(string id, AccountDeleteModel model)
        {
            if (!ModelState.IsValid) return PartialView("_Delete", model);
            if (id != model.Id) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var account = await _accountDbCommand.FindAsync(id);
            if (account == null) return HttpNotFound();
            if (account.Owner != User.Identity.Name) return new HttpUnauthorizedResult();

            Mapper.Map(model, account);

            await _accountDbCommand.DeleteAsync(account.Id);

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}