using System.Web.Mvc;
using System.Web.Routing;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;

namespace DataMigration.AdminTools.VsfDataMigrationTool.Initialization
{
    [InitializableModule]
    public class VsfDataMigrationInitialization : IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {
            RouteTable.Routes.MapRoute(
                null,
                "vsfintegration/datamigration",
                new {controller = "VsfDataMigration", action = "Index"});
        }

        public void Uninitialize(InitializationEngine context)
        {
        }
    }
}