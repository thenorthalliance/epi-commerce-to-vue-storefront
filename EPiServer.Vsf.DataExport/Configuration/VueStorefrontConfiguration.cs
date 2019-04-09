using System.Configuration;

namespace EPiServer.Vsf.DataExport.Configuration
{
    public class VueStorefrontConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("elasticServerUrls", DefaultValue = "http://127.0.0.1:9200")]
        public string ElasticServerUrls => (string) this["elasticServerUrls"];

        [ConfigurationProperty("elasticUserName", DefaultValue = "")]
        public string ElasticUserName => (string) this["elasticUserName"];

        [ConfigurationProperty("elasticPassword", DefaultValue = "")]
        public string ElasticPassword => (string) this["elasticPassword"];

        [ConfigurationProperty("indexAliasName", DefaultValue = "epi-catalog")]
        public string IndexAliasName => (string) this["indexAliasName"];

        [ConfigurationProperty("bulkIndexBatchSize", DefaultValue = 100)]
        public int BulkIndexBatchSize => (int) this["bulkIndexBatchSize"];
    }
}
