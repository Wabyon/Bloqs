using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bloqs.Data;
using Bloqs.Models;

namespace Bloqs.Controllers
{
    [Authorize]
    public class BlobController : Controller
    {
        private readonly ContainerRepository _containerRepository = new ContainerRepository(ConfigurationManager.ConnectionStrings["Default"].ConnectionString);
        private readonly BlobRepository _blobRepository = new BlobRepository(ConfigurationManager.ConnectionStrings["Default"].ConnectionString);
        
        [Route("blobs/{containerName}")]
        [HttpGet]
        public async Task<ActionResult> Index(string containerName)
        {
            var container = await _containerRepository.FindByNameAsync(containerName);
            if (container == null) return HttpNotFound();
            if (container.Owner != User.Identity.Name) return new HttpUnauthorizedResult();

            var blobs = await _blobRepository.GetAttributeListAync(container.Id, 0, 10);
            var count = await _blobRepository.CountAllAync(container.Id);

            ViewBag.ContainerName = container.Name;
            ViewBag.Skip = 0;
            ViewBag.Take = 10;
            ViewBag.TotalCount = count;
            ViewBag.HasNext = !(count <= 10);
            ViewBag.HasPreview = false;

            return View(blobs.Select(x => new BlobViewModel(x)));
        }

        [Route("blobs/{containerName}/list/{skip}/{take}")]
        [HttpGet]
        public async Task<ActionResult> GetList(string containerName, int skip, int take)
        {
            var container = await _containerRepository.FindByNameAsync(containerName);
            if (container == null) return HttpNotFound();
            if (container.Owner != User.Identity.Name) return new HttpUnauthorizedResult();

            var blobs = await _blobRepository.GetAttributeListAync(container.Id, skip, take);
            var count = await _blobRepository.CountAllAync(container.Id);

            ViewBag.ContainerName = container.Name;
            ViewBag.Skip = skip;
            ViewBag.Take = take;
            ViewBag.TotalCount = count;
            ViewBag.HasNext = !(count <= skip + take);
            ViewBag.HasPreview = (skip != 0);

            return PartialView("_List", blobs.Select(x => new BlobViewModel(x)));
        }

        [Route("blobs/{containerName}/{name}/edit")]
        [HttpGet]
        public async Task<ActionResult> Edit(string containerName, string name)
        {
            var container = await _containerRepository.FindByNameAsync(containerName);
            if (container == null) return HttpNotFound();
            if (container.Owner != User.Identity.Name) return new HttpUnauthorizedResult();

            var blob = await _blobRepository.GetAttributeAsync(container.Id, name);
            if (blob == null) return HttpNotFound();

            ViewBag.ContainerName = container.Name;
            return PartialView("_Edit", new BlobEditModel(blob));
        }

        [Route("blobs/{containerName}/{name}/edit")]
        [HttpPost]
        public async Task<ActionResult> Edit(string containerName, BlobEditModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ContainerName = containerName;
                return PartialView("_Edit", model);
            }

            if (model == null) throw new ArgumentNullException("model");

            var container = await _containerRepository.FindByNameAsync(containerName);
            if (container == null) return HttpNotFound();
            if (container.Owner != User.Identity.Name) return new HttpUnauthorizedResult();

            var target = await _blobRepository.GetAttributeAsync(container.Id, model.Name);
            if (target == null) return HttpNotFound();

            target.Properties.CacheControl = model.CacheControl;
            target.Properties.ContentDisposition = model.ContentDisposition;
            target.Properties.ContentEncoding = model.ContentEncoding;
            target.Properties.ContentLanguage = model.ContentLanguage;
            target.Properties.ContentType = model.ContentType;
            target.Properties.LastModifiedUtcDateTime = DateTime.UtcNow;
            target.Properties.ETag = CreateETag(target.Properties.LastModifiedUtcDateTime.ToString("O"));

            target.Metadata.Clear();
            foreach (var metadata in model.Metadata)
            {
                target.Metadata.Add(metadata);
            }

            await _blobRepository.UpdateAttributeAsync(container.Id, target);

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [Route("blobs/{containerName}/{name}/delete")]
        [HttpGet]
        public async Task<ActionResult> Delete(string containerName, string name)
        {
            var container = await _containerRepository.FindByNameAsync(containerName);
            if (container == null) return HttpNotFound();
            if (container.Owner != User.Identity.Name) return new HttpUnauthorizedResult();

            var blob = await _blobRepository.GetAttributeAsync(container.Id, name);
            if (blob == null) return HttpNotFound();

            ViewBag.ContainerName = container.Name;
            return PartialView("_Delete", new BlobViewModel(blob));
        }

        [Route("blobs/{containerName}/{name}/delete")]
        [HttpPost]
        public async Task<ActionResult> DeleteBlob(string containerName, string name)
        {
            var container = await _containerRepository.FindByNameAsync(containerName);
            if (container == null) return HttpNotFound();
            if (container.Owner != User.Identity.Name) return new HttpUnauthorizedResult();

            var target = await _blobRepository.GetAttributeAsync(container.Id, name);
            if (target == null) return HttpNotFound();

            await _blobRepository.DeleteAsync(container.Id, name);

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [Route("blobs/{containerName}/{name}/download")]
        [HttpGet]
        public async Task<ActionResult> Download(string containerName, string name)
        {
            var container = await _containerRepository.FindByNameAsync(containerName);
            if (container == null) return HttpNotFound();
            if (container.Owner != User.Identity.Name) return new HttpUnauthorizedResult();

            var target = await _blobRepository.GetAsync(container.Id, name);
            if (target == null) return HttpNotFound();

            return File(target.Image, target.Properties.ContentType, target.Name);
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