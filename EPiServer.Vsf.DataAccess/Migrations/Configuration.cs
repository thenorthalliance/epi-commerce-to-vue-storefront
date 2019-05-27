using System.Data.Entity.Migrations;

namespace EPiServer.Vsf.DataAccess.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<QuicksilverDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(QuicksilverDbContext context)
        {
        }
    }
}
