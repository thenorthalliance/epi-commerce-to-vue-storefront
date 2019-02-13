using Newtonsoft.Json;

namespace DataMigration.Output.ElasticSearch.Entity.Product.Model
{
    public class Stock
    {
        [JsonProperty("is_in_stock")]
        public bool IsInStock { get; set; }

        [JsonProperty("qty")]
        public int Quantity { get; set; }
    }
}