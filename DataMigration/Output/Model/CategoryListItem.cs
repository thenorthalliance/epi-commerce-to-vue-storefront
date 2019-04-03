using Nest;

namespace DataMigration.Output.Model
{
    public class CategoryListItem
    {
        [PropertyName("category_id")]
        public int Id { get; set; }

        [PropertyName("name")]
        public string Name { get; set; }
    }
}