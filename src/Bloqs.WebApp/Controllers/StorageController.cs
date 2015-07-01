using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using Bloqs.BlobStorage.Data;
using Bloqs.Data.Commands;
using Bloqs.Filters;
using Bloqs.Models;

namespace Bloqs.Controllers
{
    [RoutePrefix("storages")]
    [Authorize]
    public class StorageController : Controller
    {
        private readonly StorageDbCommand _storageDbCommand = new StorageDbCommand(ConfigurationManager.ConnectionStrings["Default"].ConnectionString);
        private readonly AccountDbCommand _accountDbCommand = new AccountDbCommand(ConfigurationManager.ConnectionStrings["Default"].ConnectionString);

        [NoCache]
        [Route("")]
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var count = await _storageDbCommand.CountByOwnerAsync(User.Identity.Name);
            var storages = count == 0 ? new Storage[0] : (await _storageDbCommand.GetListByOwnerAsync(User.Identity.Name, 0, 10)).ToArray();

            ViewBag.Skip = 0;
            ViewBag.Take = 10;
            ViewBag.TotalCount = count;
            ViewBag.HasNext = !(count <= 10);
            ViewBag.HasPreview = false;

            return View(Mapper.Map<IEnumerable<StorageIndexModel>>(storages));
        }

        [NoCache]
        [Route("list")]
        [HttpGet]
        public async Task<ActionResult> List(int skip, int take)
        {
            var count = await _storageDbCommand.CountByOwnerAsync(User.Identity.Name);
            var storages = count == 0 ? new Storage[0] : (await _storageDbCommand.GetListByOwnerAsync(User.Identity.Name, skip, take)).ToArray();

            ViewBag.Skip = skip;
            ViewBag.Take = take;
            ViewBag.TotalCount = count;
            ViewBag.HasNext = !(count <= skip + take);
            ViewBag.HasPreview = (skip != 0);

            return PartialView("_List", Mapper.Map<IEnumerable<StorageIndexModel>>(storages));
        }

        [Route("{id}/detail")]
        [HttpGet]
        public async Task<ActionResult> Detail(string id)
        {
            var storage = await _storageDbCommand.FindAsync(id);
            if (storage == null) return HttpNotFound();
            if (storage.Owner != User.Identity.Name) return new HttpUnauthorizedResult();

            return PartialView("_Detail", Mapper.Map<StorageIndexModel>(storage));
        }

        [Route("personal/new")]
        [HttpGet]
        public ActionResult CreatePersonal()
        {
            return View();
        }

        [Route("personal/new")]
        [HttpPost]
        public async Task<ActionResult> CreatePersonal(PersonalStorageCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var newStorage = Storage.NewStorage(model.Name, User.Identity.Name, StorageType.Personal);

            Mapper.Map(model, newStorage);

            try
            {
                BlobStorageConfig.Initialize(newStorage);
            }
            catch (SqlException exception)
            {
                ModelState.AddModelError("", exception.Message);
                return View(model);
            }

            await _storageDbCommand.CreateAsync(newStorage);

            return RedirectToAction("Index");
        }

        [Route("{id}/edit")]
        [HttpGet]
        public async Task<ActionResult> Edit(string id)
        {
            var storage = await _storageDbCommand.FindAsync(id);
            if (storage == null) return HttpNotFound();
            if (storage.Owner != User.Identity.Name) return new HttpUnauthorizedResult();

            if (storage.StorageType == StorageType.Common)
            {
                return View("EditCommon");
            }

            var model = Mapper.Map<PersonalStorageEditModel>(storage);
            model.IsAlreadyUsed = (await _accountDbCommand.GetListByStorageAsync(storage, 0, 1)).Any();

            return View("EditPersonal", model);
        }

        [Route("personal/{id}/edit")]
        [HttpPost]
        public async Task<ActionResult> EditPersonal(string id, PersonalStorageEditModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var storage = await _storageDbCommand.FindAsync(id);
            if (storage == null) return HttpNotFound();

            if (storage.Owner != User.Identity.Name) return new HttpUnauthorizedResult();

            Mapper.Map(model, storage);
            storage.LastModifiedUtcDateTime = DateTime.UtcNow;

            try
            {
                BlobStorageConfig.Initialize(storage);
            }
            catch (SqlException exception)
            {
                ModelState.AddModelError("", exception.Message);
                return View(model);
            }

            await _storageDbCommand.UpdateAsync(id, storage);

            return RedirectToAction("Index");
        }

        [Route("{id}/delete")]
        [HttpGet]
        public async Task<ActionResult> Delete(string id)
        {
            var storage = await _storageDbCommand.FindAsync(id);
            if (storage == null) return HttpNotFound();
            if (storage.Owner != User.Identity.Name) return new HttpUnauthorizedResult();

            if (storage.StorageType == StorageType.Common)
            {
                return View("DeleteCommon");
            }

            var model = Mapper.Map<PersonalStorageDeleteModel>(storage);
            model.IsAlreadyUsed = (await _accountDbCommand.GetListByStorageAsync(storage, 0, 1)).Any();

            return View("DeletePersonal", Mapper.Map<PersonalStorageDeleteModel>(storage));
        }

        [Route("personal/{id}/delete")]
        [HttpPost]
        public async Task<ActionResult> DeletePersonal(string id, PersonalStorageDeleteModel model)
        {
            var storage = await _storageDbCommand.FindAsync(id);
            if (storage == null) return HttpNotFound();
            if (storage.Owner != User.Identity.Name) return new HttpUnauthorizedResult();

            if ((await _accountDbCommand.GetListByStorageAsync(storage, 0, 1)).Any())
            {
                ModelState.AddModelError("", @"アカウントに利用されているため、削除できません。");
                return View(model);
            }

            await _storageDbCommand.DeleteAsync(storage.Id);

            return RedirectToAction("Index");
        }
    }
}