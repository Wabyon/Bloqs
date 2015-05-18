using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bloqs.Data;
using Bloqs.Models;

namespace Bloqs.Controllers
{
    [Authorize]
    public class ContainerController : Controller
    {
        private readonly ContainerRepository _containerRepository = new ContainerRepository(ConfigurationManager.ConnectionStrings["Default"].ConnectionString);

        [Route("")]
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var containers = await _containerRepository.GetListByOwnerAsync(User.Identity.Name, 0, 10);
            var count = await _containerRepository.CountByOwnerAsync(User.Identity.Name);

            ViewBag.Skip = 0;
            ViewBag.Take = 10;
            ViewBag.TotalCount = count;
            ViewBag.HasNext = !(count <= 10);
            ViewBag.HasPreview = false;

            return View(containers.Select(x => new ContainerViewModel(x)));
        }

        [Route("container/list/{skip}/{take}")]
        [HttpGet]
        public async Task<ActionResult> GetList(int skip, int take)
        {
            var containers = await _containerRepository.GetListByOwnerAsync(User.Identity.Name, skip, take);
            var count = await _containerRepository.CountByOwnerAsync(User.Identity.Name);

            ViewBag.Skip = skip;
            ViewBag.Take = take;
            ViewBag.TotalCount = count;
            ViewBag.HasNext = !(count <= skip + take);
            ViewBag.HasPreview = (skip != 0);

            return PartialView("_List", containers.Select(x => new ContainerViewModel(x)));
        }

        [Route("container/new")]
        [HttpGet]
        public ActionResult Create()
        {
            return PartialView("_Create", new ContainerCreateModel());
        }

        [Route("container/new")]
        [HttpPost]
        public async Task<ActionResult> Create(ContainerCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_Create", model);
            }

            if (!Container.IsUsableName(model.Name))
            {
                ModelState.AddModelError("Name", "This name is unusable.");
                return PartialView("_Create", model);
            }

            if (await _containerRepository.ExistsNameAsync(model.Name))
            {
                ModelState.AddModelError("Name", "This name is already used or unusable.");
                return PartialView("_Create", model);
            }

            await _containerRepository.CreateAsync(model.ToContainer(User.Identity.Name));

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [Route("container/{name}/edit")]
        [HttpGet]
        public async Task<ActionResult> Edit(string name)
        {
            var container = await _containerRepository.FindByNameAsync(name);
            if (container == null) return HttpNotFound();
            if (container.Owner != User.Identity.Name) return new HttpUnauthorizedResult();

            return PartialView("_Edit",new ContainerEditModel{Name = container.Name, IsPublic = container.IsPublic});
        }

        [Route("container/{name}/edit")]
        [HttpPost]
        public async Task<ActionResult> Edit(string name, ContainerEditModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_Edit", model);
            }

            var container = await _containerRepository.FindByNameAsync(name);
            if (container == null) return HttpNotFound();
            if (container.Owner != User.Identity.Name) return new HttpUnauthorizedResult();

            container.IsPublic = model.IsPublic;
            container.LastModifiedUtcDateTime = DateTime.UtcNow;

            await _containerRepository.UpdateAsync(container.Id, container);

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [Route("container/{name}/delete")]
        [HttpGet]
        public async Task<ActionResult> Delete(string name)
        {
            var container = await _containerRepository.FindByNameAsync(name);
            if (container == null) return HttpNotFound();
            if (container.Owner != User.Identity.Name) return new HttpUnauthorizedResult();

            var model = new ContainerDeleteModel(container);

            return PartialView("_Delete", model);
        }

        [Route("container/{name}/delete")]
        [HttpPost]
        public async Task<ActionResult> Delete(string name, ContainerDeleteModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_Delete", model);
            }

            var container = await _containerRepository.FindByNameAsync(name);
            if (container == null) return HttpNotFound();
            if (container.Owner != User.Identity.Name) return new HttpUnauthorizedResult();

            await _containerRepository.DeleteAsync(container.Id);

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [Route("container/{name}/keys")]
        [HttpGet]
        public async Task<ActionResult> MaintainKey(string name)
        {
            var container = await _containerRepository.FindByNameAsync(name);
            if (container == null) return HttpNotFound();
            if (container.Owner != User.Identity.Name) return new HttpUnauthorizedResult();

            return PartialView("_MaintainKey",
                new ContainerKeyEditModel
                {
                    Name = container.Name,
                    PrimaryAccessKey = container.PrimaryAccessKey,
                    SecondaryAccessKey = container.SecondaryAccessKey
                });
        }

        [Route("container/{name}/keys/recreate")]
        [HttpPost]
        public async Task<ActionResult> RecreateAccessKey(string name, int keyno)
        {
            if (keyno < 1 || keyno > 2) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var container = await _containerRepository.FindByNameAsync(name);
            if (container == null) return HttpNotFound();
            if (container.Owner != User.Identity.Name) return new HttpUnauthorizedResult();

            var newkey = ContainerKeyEditModel.CreateNewAccessKeyString();

            if (keyno == 1) container.PrimaryAccessKey = newkey;
            else container.SecondaryAccessKey = newkey;

            container.LastModifiedUtcDateTime = DateTime.UtcNow;

            await _containerRepository.UpdateAsync(container.Id, container);

            return
                Json(new ContainerKeyEditModel
                {
                    Name = container.Name,
                    PrimaryAccessKey = container.PrimaryAccessKey,
                    SecondaryAccessKey = container.SecondaryAccessKey
                });
        }
    }
}