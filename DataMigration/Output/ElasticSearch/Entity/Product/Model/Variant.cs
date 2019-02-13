using EPiServer.Commerce.Catalog.ContentTypes;
using Newtonsoft.Json;

namespace DataMigration.Output.ElasticSearch.Entity.Product.Model
{
    public class Variant : ProductBase
    {
        public Variant(VariationContent variation, int productId) : base(variation)
        {
            ProductId = productId;
        }

        [JsonProperty("product_id")]
        public int ProductId { get; set; }
    }
}
