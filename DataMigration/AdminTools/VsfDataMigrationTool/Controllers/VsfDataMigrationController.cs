using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DataMigration.AdminTools.VsfDataMigrationTool.ViewModels;
using DataMigration.Input.Episerver;
using DataMigration.Input.Episerver.Category.Service;
using DataMigration.Mapper;
using DataMigration.Output.ElasticSearch.Entity;
using EPiServer;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.Globalization;
using EPiServer.PlugIn;
using EPiServer.ServiceLocation;
using Mediachase.Commerce.Catalog;

namespace DataMigration.AdminTools.VsfDataMigrationTool.Controllers
{
    [GuiPlugIn(
        Area = PlugInArea.AdminMenu,
        Url = "/vsfintegration/datamigration",
        DisplayName = "DataMigration for Vuestorefront")]
    [Authorize(Roles = "CmsAdmins")]
    public class VsfDataMigrationController : Controller
    {
        private readonly IContentLoader _contentLoader = ServiceLocator.Current.GetInstance<IContentLoader>();
        private readonly ReferenceConverter _referenceConverter = ServiceLocator.Current.GetInstance<ReferenceConverter>();
        public ActionResult Index()
        {
            var model = new VsfDataMigrationViewModel {Text = "Data migration's admin plugin for Vue Store Front"};
            return View(model);
        }

        public ActionResult MigrateData()
        {
            var catalogs = _contentLoader.GetChildren<CatalogContent>(_referenceConverter.GetRootLink()).ToList();
            var mappedCategories = GetMappedCategories(catalogs[0].ContentLink);

            return Json(mappedCategories, JsonRequestBehavior.AllowGet);
        }

        private static IEnumerable<Entity> GetMappedCategories(ContentReference catalogReference)
        {
            var categoryMapper = MapperFactory.Create(EntityType.Category);
            var categoryService = ContentServiceFactory.Create(EntityType.Category);
            var categories = categoryService.GetAll(catalogReference, ContentLanguage.PreferredCulture, 2);
            var mapped = categories.Select(category => categoryMapper.Map(category)).ToList();
            return mapped;
        }
    }
}