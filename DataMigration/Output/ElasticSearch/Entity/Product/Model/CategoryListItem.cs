using Nest;

namespace DataMigration.Output.ElasticSearch.Entity.Product.Model
{
    public class CategoryListItem
    {
        [PropertyName("category_id")]
        public int Id { get; set; }

        [PropertyName("name")]
        public string Name { get; set; }
    }
}