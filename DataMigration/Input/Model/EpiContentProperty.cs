using System.Collections.Generic;

namespace DataMigration.Input.Model
{
    public class EpiContentProperty : ICmsObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<string> Values { get; set; }
    }
}