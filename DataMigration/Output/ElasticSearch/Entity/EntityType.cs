using System;
using System.Linq;
using System.Reflection;

namespace DataMigration.Output.ElasticSearch.Entity
{
    public enum EntityType
    {
        [Display(Name = "product")]
        Product,

        [Display(Name = "category")]
        Category,

        [Display(Name = "attribute")]
        Attribute
    }

    public static class EnumExtensions {
        public static string DisplayName(this Enum item)
        {
            return item.GetType()?.GetMember(item.ToString()).First()?.GetCustomAttribute<DisplayAttribute>()?.Name;
        }
    }

    class DisplayAttribute : System.Attribute
    {
        public string Name { get; set; }
    }
    
}