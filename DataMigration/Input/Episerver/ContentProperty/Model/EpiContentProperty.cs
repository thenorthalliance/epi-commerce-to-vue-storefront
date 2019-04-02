using System.Collections.Generic;
using DataMigration.Input.Episerver.Common.Model;

namespace DataMigration.Input.Episerver.ContentProperty.Model
{
    public class EpiContentProperty : ICmsObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<string> Values { get; set; }
    }
}