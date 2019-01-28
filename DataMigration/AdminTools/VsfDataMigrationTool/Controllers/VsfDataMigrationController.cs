using System.Web.Mvc;
using DataMigration.AdminTools.VsfDataMigrationTool.ViewModels;
using EPiServer.PlugIn;

namespace DataMigration.AdminTools.VsfDataMigrationTool.Controllers
{
    [GuiPlugIn(
        Area = PlugInArea.AdminMenu,
        Url = "/vsfintegration/datamigration",
        DisplayName = "DataMigration for Vuestorefront")]
    [Authorize(Roles = "CmsAdmins")]
    public class VsfDataMigrationController : Controller
    {
        public ActionResult Index()
        {
            var model = new VsfDataMigrationViewModel {Text = "Data migration's admin plugin for Vue Store Front"};
            return View(model);
        }
    }
}