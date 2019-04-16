using Nest;

namespace EPiServer.Vsf.DataExport.Model
{
    public class CategoryListItem
    {
        [PropertyName("category_id")]
        public int Id { get; set; }

        [PropertyName("name")]
        public string Name { get; set; }
    }
}