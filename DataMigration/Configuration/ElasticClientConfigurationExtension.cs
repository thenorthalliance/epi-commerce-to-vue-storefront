using System;
using System.Linq;
using Elasticsearch.Net;
using Nest;

namespace DataMigration.Configuration
{
    public static class ElasticClientConfigurationExtension
    {
        public static IConnectionSettingsValues GetElasticConnectionSettings(this VueStorefrontConfiguration config)
        {
            return GetElasticConnectionSettings(config.ElasticServerUrls, config.ElasticUserName, config.ElasticPassword);
        }

        public static IConnectionSettingsValues GetElasticConnectionSettings(string elasticServerUrls, string elasticUserName = null, string elasticPassword = null)
        {
            var nodes = elasticServerUrls.Split(';').Select(x => new Uri(x));
            var connectionPool = new StaticConnectionPool(nodes);
            var connectionSettings = new ConnectionSettings(connectionPool);

            if (!string.IsNullOrEmpty(elasticUserName) || !string.IsNullOrEmpty(elasticPassword))
            {
                connectionSettings.BasicAuthentication(elasticUserName, elasticPassword);
            }

            return connectionSettings;
        }
    }
}
