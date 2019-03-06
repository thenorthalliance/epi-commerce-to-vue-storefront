using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using DataMigration.AdminTools.VsfDataMigrationTool.ViewModels;
using DataMigration.Input.Episerver;
using DataMigration.Input.Episerver.Common.Model;
using DataMigration.Mapper;
using DataMigration.Output.ElasticSearch.Entity;
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
        public ActionResult Index()
        {
            var model = new VsfDataMigrationViewModel {Text = "Data migration's admin plugin for Vue Store Front"};
            return View(model);
        }

        public async Task<ActionResult> MigrateAttributes()
        {
            return Content(await MigrateEntities(EntityType.Attribute), "application/json", Encoding.UTF8);
        }

        public async Task<ActionResult> MigrateCategories()
        {
            return Content(await MigrateEntities(EntityType.Category), "application/json", Encoding.UTF8);
        }

        public async Task<ActionResult> MigrateProducts()
        {
            return Content(await MigrateEntities(EntityType.Product), "application/json", Encoding.UTF8);
        }

        public ActionResult GetCategories()
        {
            var catalogs = _contentLoader.GetChildren<CatalogContent>(_referenceConverter.GetRootLink()).ToList();
            var mappedCategories = GetMappedEntites(catalogs[0].ContentLink, EntityType.Category);
            var json = JsonConvert.SerializeObject(mappedCategories);
            return Content(json, "application/json", Encoding.UTF8);
        }
        public ActionResult GetProducts()
        {
            var catalogs = _contentLoader.GetChildren<CatalogContent>(_referenceConverter.GetRootLink()).ToList();
            var mappedProducts = GetMappedEntites(catalogs[0].ContentLink, EntityType.Product);
            var json = JsonConvert.SerializeObject(mappedProducts);
            return Content(json, "application/json", Encoding.UTF8);
        }
        public ActionResult GetAttributes()
        {
            var catalogs = _contentLoader.GetChildren<CatalogContent>(_referenceConverter.GetRootLink()).ToList();
            var mappedAttributes = GetMappedEntites(catalogs[0].ContentLink, EntityType.Attribute);
            var json = JsonConvert.SerializeObject(mappedAttributes);
            return Content(json, "application/json", Encoding.UTF8);
        }

        private async Task<string> MigrateEntities(EntityType entityType)
        {
            var catalogs = _contentLoader.GetChildren<CatalogContent>(_referenceConverter.GetRootLink()).ToList();
            var products = GetMappedEntites(catalogs[0].ContentLink, entityType);
            var result = await Migrate(products, entityType.DisplayName());
            return JsonConvert.SerializeObject(result);
        }

        private static async Task<dynamic[]> Migrate(IEnumerable<Entity> entities, string type)
        {
            var indexService = new IndexApiService("epi_catalog");
            return await indexService.SendAsync(entities, type);
        }

        private static IEnumerable<Entity> GetMappedEntites(ContentReference catalogReference, EntityType entityType)
        {
            var mapper = MapperFactory.Create(entityType);
            return GetEntites(catalogReference, entityType).Select(mapper.Map);
        }

        private static IEnumerable<CmsObjectBase> GetEntites(ContentReference catalogReference, EntityType entityType)
        {
            var contentService = ContentServiceFactory.Create(entityType);
            var content = contentService.GetAll(catalogReference, ContentLanguage.PreferredCulture);
            return content;
        }
    }
}