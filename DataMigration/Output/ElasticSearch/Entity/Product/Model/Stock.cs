using Nest;

namespace DataMigration.Output.ElasticSearch.Entity.Product.Model
{
    public class Stock
    {
        [PropertyName("is_in_stock")]
        public bool IsInStock { get; set; }

        [PropertyName("qty")]
        public int Quantity { get; set; }
    }
}