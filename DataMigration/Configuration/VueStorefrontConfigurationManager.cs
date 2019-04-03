using System.Configuration;

namespace DataMigration.Configuration
{
    public static class VueStorefrontConfigurationManager
    {
        public static VueStorefrontConfiguration Configuration
        {
            get
            {
                var configuration = ConfigurationManager.GetSection("vueStorefront") as VueStorefrontConfiguration;
                return configuration ?? new VueStorefrontConfiguration();
            }
        }

    }
}
