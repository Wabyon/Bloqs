using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Bloqs.Data;
using Bloqs.Data.Commands;
using Bloqs.Filters;
using Bloqs.Models;

namespace Bloqs.Controllers
{
    [RoutePrefix("accounts/{accountName}/containers/{containerName}/blobs")]
    [Authorize]
    public class BlobController : Controller
    {
        private readonly AccountDbCommand _accountDbCommand =
            new AccountDbCommand(ConfigurationManager.ConnectionStrings["Default"].ConnectionString);

        private readonly ContainerDbCommand _containerDbCommand =
            new ContainerDbCommand(ConfigurationManager.ConnectionStrings["Default"].ConnectionString);

        private readonly BlobDbCommand _blobDbCommand =
            new BlobDbCommand(ConfigurationManager.ConnectionStrings["Default"].ConnectionString);

        [NoCache]
        [Route("")]
        [HttpGet]
        public async Task<ActionResult> Index(string accountName, string containerName)
        {
            var account = await _accountDbCommand.FindByNameAsync(accountName);
            if (account == null) return HttpNotFound();
            if (account.Owner != User.Identity.Name) return new HttpUnauthorizedResult();

            var container = await _containerDbCommand.FindAsync(account, containerName);
            if (container == null) return HttpNotFound();

            var count = await _blobDbCommand.CountAsync(container);
            var blobs = await _blobDbCommand.GetAttributesListAsync(container, 0, 10);

            ViewBag.AccountName = container.Account.Name;
            ViewBag.ContainerName = container.Name;
            ViewBag.Skip = 0;
            ViewBag.Take = 10;
            ViewBag.TotalCount = count;
            ViewBag.HasNext = !(count <= 10);
            ViewBag.HasPreview = false;

            return View(Mapper.Map<IEnumerable<BlobIndexModel>>(blobs));
        }

        [NoCache]
        [Route("list")]
        [HttpGet]
        public async Task<ActionResult> List(string accountName, string containerName, int skip, int take)
        {
            var account = await _accountDbCommand.FindByNameAsync(accountName);
            if (account == null) return HttpNotFound();
            if (account.Owner != User.Identity.Name) return new HttpUnauthorizedResult();

            var container = await _containerDbCommand.FindAsync(account, containerName);
            if (container == null) return HttpNotFound();

            var count = await _blobDbCommand.CountAsync(container);
            var blobs = await _blobDbCommand.GetAttributesListAsync(container, skip, take);

            ViewBag.Skip = skip;
            ViewBag.Take = take;
            ViewBag.TotalCount = count;
            ViewBag.HasNext = !(count <= skip + take);
            ViewBag.HasPreview = (skip != 0);

            return PartialView("_List", Mapper.Map<IEnumerable<BlobIndexModel>>(blobs));
        }

        [NoCache]
        [Route("upload")]
        [HttpGet]
        public async Task<ActionResult> Upload(string accountName, string containerName)
        {
            var account = await _accountDbCommand.FindByNameAsync(accountName);
            if (account == null) return HttpNotFound();
            if (account.Owner != User.Identity.Name) return new HttpUnauthorizedResult();

            var container = await _containerDbCommand.FindAsync(account, containerName);
            if (container == null) return HttpNotFound();

            ViewBag.AccountName = container.Account.Name;
            ViewBag.ContainerName = container.Name;
            return View("Upload");
        }

        [NoCache]
        [Route("upload")]
        [HttpPost]
        public async Task<ActionResult> UploadImage(string accountName, string containerName)
        {
            ViewBag.AccountName = accountName;
            ViewBag.ContainerName = containerName;

            if (Request.Files.Count == 0) return View("Upload");
            var file = Request.Files[0];
            if (file == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            if (string.IsNullOrWhiteSpace(file.FileName)) return View("Upload");

            var account = await _accountDbCommand.FindByNameAsync(accountName);
            if (account == null) return HttpNotFound();
            if (account.Owner != User.Identity.Name) return new HttpUnauthorizedResult();

            var container = await _containerDbCommand.FindAsync(account, containerName);
            if (container == null) return HttpNotFound();

            var image = new byte[file.ContentLength];
            var fileName = new FileInfo(file.FileName).Name;
            var contentType = MimeMapping.GetMimeMapping(fileName);
            await file.InputStream.ReadAsync(image, 0, file.ContentLength);

            var attributes = await _blobDbCommand.GetAttributesAsync(container, fileName);

            Blob blob;

            if (attributes != null)
            {
                blob = new Blob(attributes)
                {
                    Image = image,
                    Properties =
                    {
                        ContentType = contentType,
                        LastModifiedUtcDateTime = DateTime.UtcNow,
                        Length = file.ContentLength
                    },
                };
            }
            else
            {
                blob = new Blob(container.CreateBlobAttributes(fileName))
                {
                    Image = image,
                    Properties =
                    {
                        ContentType = contentType,
                        LastModifiedUtcDateTime = DateTime.UtcNow,
                        Length = file.ContentLength
                    },
                };
            }

            await _blobDbCommand.SaveAsync(blob);

            return RedirectToAction("Index");
        }

        [NoCache]
        [Route("{id}/detail")]
        [HttpGet]
        public async Task<ActionResult> Detail(string accountName, string containerName, string id)
        {
            var account = await _accountDbCommand.FindByNameAsync(accountName);
            if (account == null) return HttpNotFound();
            if (account.Owner != User.Identity.Name) return new HttpUnauthorizedResult();

            var container = await _containerDbCommand.FindAsync(account, containerName);
            if (container == null) return HttpNotFound();

            var blob = await _blobDbCommand.GetAttributesAsync(id);
            if (blob == null) return HttpNotFound();

            ViewBag.ContainerName = container.Name;
            return PartialView("_Detail", Mapper.Map<BlobIndexModel>(blob));
        }

        [NoCache]
        [Route("{id}/edit")]
        [HttpGet]
        public async Task<ActionResult> Edit(string accountName, string containerName, string id)
        {
            var account = await _accountDbCommand.FindByNameAsync(accountName);
            if (account == null) return HttpNotFound();
            if (account.Owner != User.Identity.Name) return new HttpUnauthorizedResult();

            var container = await _containerDbCommand.FindAsync(account, containerName);
            if (container == null) return HttpNotFound();

            var blob = await _blobDbCommand.GetAttributesAsync(id);
            if (blob == null) return HttpNotFound();

            ViewBag.ContainerName = container.Name;
            return PartialView("_Edit", Mapper.Map<BlobEditModel>(blob));
        }

        [NoCache]
        [Route("{id}/edit")]
        [HttpPost]
        public async Task<ActionResult> Edit(string accountName, string containerName, string id, BlobEditModel model)
        {
            var account = await _accountDbCommand.FindByNameAsync(accountName);
            if (account == null) return HttpNotFound();
            if (account.Owner != User.Identity.Name) return new HttpUnauthorizedResult();

            var container = await _containerDbCommand.FindAsync(account, containerName);
            if (container == null) return HttpNotFound();

            if (!ModelState.IsValid) return PartialView("_Edit", model);

            var target = await _blobDbCommand.GetAttributesAsync(id);
            if (target == null) return HttpNotFound();

            Mapper.Map(model, target);
            target.Properties.LastModifiedUtcDateTime = DateTime.UtcNow;

            await _blobDbCommand.UpdateAttributesAsync(target);

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [NoCache]
        [Route("{id}/delete")]
        [HttpGet]
        public async Task<ActionResult> Delete(string accountName, string containerName, string id)
        {
            var account = await _accountDbCommand.FindByNameAsync(accountName);
            if (account == null) return HttpNotFound();
            if (account.Owner != User.Identity.Name) return new HttpUnauthorizedResult();

            var container = await _containerDbCommand.FindAsync(account, containerName);
            if (container == null) return HttpNotFound();

            var blob = await _blobDbCommand.GetAttributesAsync(id);
            if (blob == null) return HttpNotFound();

            return PartialView("_Delete", Mapper.Map<BlobDeleteModel>(blob));
        }

        [NoCache]
        [Route("{id}/delete")]
        [HttpPost]
        public async Task<ActionResult> Delete(string accountName, string containerName, string id, BlobDeleteModel model)
        {
            var account = await _accountDbCommand.FindByNameAsync(accountName);
            if (account == null) return HttpNotFound();
            if (account.Owner != User.Identity.Name) return new HttpUnauthorizedResult();

            var container = await _containerDbCommand.FindAsync(account, containerName);
            if (container == null) return HttpNotFound();

            if (!ModelState.IsValid) return PartialView("_Delete", model);

            var target = await _blobDbCommand.GetAttributesAsync(id);
            if (target == null) return HttpNotFound();

            await _blobDbCommand.DeleteAsync(id);

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [NoCache]
        [Route("{id}/download")]
        [HttpGet]
        public async Task<ActionResult> Download(string accountName, string containerName, string id)
        {
            var account = await _accountDbCommand.FindByNameAsync(accountName);
            if (account == null) return HttpNotFound();
            if (account.Owner != User.Identity.Name) return new HttpUnauthorizedResult();

            var container = await _containerDbCommand.FindAsync(account, containerName);
            if (container == null) return HttpNotFound();

            var target = await _blobDbCommand.FindAsync(id);
            if (target == null) return HttpNotFound();

            var contentType = target.Properties.ContentType ?? "application/octet-stream";

            return File(target.Image, contentType, target.Name);
        }
    }
}