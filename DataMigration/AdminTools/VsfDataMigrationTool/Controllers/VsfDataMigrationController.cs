using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using DataMigration.AdminTools.VsfDataMigrationTool.ViewModels;
using DataMigration.Input.Episerver;
using DataMigration.Input.Episerver.Common.Model;
using DataMigration.Mapper;
using DataMigration.Output.ElasticSearch.Entity.Attribute.Model;
using DataMigration.Output.ElasticSearch.Entity.Category.Model;
using DataMigration.Output.ElasticSearch.Entity.Product.Model;
using DataMigration.Output.ElasticSearch.Service;
using EPiServer;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.Globalization;
using EPiServer.PlugIn;
using EPiServer.ServiceLocation;
using Mediachase.Commerce.Catalog;
using Newtonsoft.Json;

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
        private readonly IndexApiService _indexService = new IndexApiService("epi_catalog4", "http://localhost:9200");

        public ActionResult Index()
        {
            var model = new VsfDataMigrationViewModel {Text = "Data migration's admin plugin for Vue Store Front"};
            return View(model);
        }

        public async Task<ActionResult> MigrateAll()
        {
            await _indexService.CreateIndex();

            var results = await Task.WhenAll(
                MigrateEntities<Attribute>(),
                MigrateEntities<Category>(),
                MigrateEntities<Product>());

            return Json(results, JsonRequestBehavior.AllowGet);
        }

//        public async Task<ActionResult> MigrateAttributes()
//        {
//            return Content(await MigrateEntities<Attribute>(), "application/json", Encoding.UTF8);
//        }
//
//        public async Task<ActionResult> MigrateCategories()
//        {
//            return Content(await MigrateEntities<Category>(), "application/json", Encoding.UTF8);
//        }
//
//        public async Task<ActionResult> MigrateProducts()
//        {
//            return Content(await MigrateEntities<Product>(), "application/json", Encoding.UTF8);
//        }

        public ActionResult GetCategories()
        {
            var catalogs = _contentLoader.GetChildren<CatalogContent>(_referenceConverter.GetRootLink()).ToList();
            var mappedCategories = GetMappedEntites<Category>(catalogs[0].ContentLink);
            return new FileStreamResult(_indexService.Serialize(mappedCategories), "application/json");
        }

        public ActionResult GetProducts()
        {
            var catalogs = _contentLoader.GetChildren<CatalogContent>(_referenceConverter.GetRootLink()).ToList();
            var mappedProducts = GetMappedEntites<Product>(catalogs[0].ContentLink);
            return new FileStreamResult(_indexService.Serialize(mappedProducts), "application/json");
        }

        public ActionResult GetAttributes()
        {
            var catalogs = _contentLoader.GetChildren<CatalogContent>(_referenceConverter.GetRootLink()).ToList();
            var mappedAttributes = GetMappedEntites<Attribute>(catalogs[0].ContentLink);
            return new FileStreamResult(_indexService.Serialize(mappedAttributes), "application/json");
        }

        private async Task<dynamic[]> MigrateEntities<T>() where T:class
        {
            var catalogs = _contentLoader.GetChildren<CatalogContent>(_referenceConverter.GetRootLink()).ToList();
            var products = GetMappedEntites<T>(catalogs[0].ContentLink);
            return await Migrate(products);
        }

        
        private async Task<dynamic[]> Migrate<T>(IEnumerable<T> entities) where T: class
        {
            return await _indexService.IndexMany(entities);
        }

        private static IEnumerable<T> GetMappedEntites<T>(ContentReference catalogReference) where T: class
        {
            var mapper = MapperFactory.Create<T>();
            return GetEntites<T>(catalogReference).Select(mapper.Map);
        }

        private static IEnumerable<CmsObjectBase> GetEntites<T>(ContentReference catalogReference) where T:class
        {
            var contentService = ContentServiceFactory.Create<T>();
            var content = contentService.GetAll(catalogReference, ContentLanguage.PreferredCulture);
            return content;
        }
    }
}