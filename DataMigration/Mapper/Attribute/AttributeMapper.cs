using System.Linq;
using DataMigration.Input.Episerver.Attribute.Model;
using DataMigration.Input.Episerver.Common.Model;
using DataMigration.Output.ElasticSearch.Entity;
using DataMigration.Output.ElasticSearch.Entity.Attribute.Helper;
using DataMigration.Output.ElasticSearch.Entity.Attribute.Model;

namespace DataMigration.Mapper.Attribute
{
    public class AttributeMapper : IMapper
    {
        public Entity Map(CmsObjectBase cmsObject)
        {
            if (!(cmsObject is EpiAttribute source))
            {
                return null;
            }
            return new Output.ElasticSearch.Entity.Attribute.Model.Attribute()
            {
                Id = source.Id,
                EntityTypeId = 4,
                AttributeModel = null,
                BackendModel = null,
                BackendType = "int",
                BackendTable = null,
                FrontendModel = null,
                FrontendClass = null,
                SourceModel = "eav/entity_attribute_source_table",
                IsRequired = false,
                IsUserDefined = true,
                DefaultValue = "",
                IsUnique = false,
                Note = null,
                AttributeId = source.Id,
                FrontendInputRenderer = null,
                IsGlobal = true,
                IsVisible = true,
                IsSearchable = true,
                IsFilterable = true,
                IsComparable = false,
                IsVisibleOnFront = false,
                IsHtmlAllowedOnFront = true,
                IsUsedForPriceRules = false,
                IsFilterableInSearch = false,
                UsedInProductListing = 0,
                UsedForSortBy = 0,
                IsConfigurable = true,
                ApplyTo = new []{"simple","grouped","configurable"},
                IsVisibleInAdvancedSearch = false,
                Position = 0,
                IsWysiwygEnabled = false,
                IsUsedForPromoRules = false,
                SearchWeight = 1,
                FrontendInput = "select",
                Name = source.Name,
                FrontendLabel = source.Name,
                AttributeCode = source.Name.Replace(" ","_").ToLower(),
                Options = source.Values.Select(x => new Option {Name = x, Value = AttributeHelper.CreateValueIndex(source.Id, x)})
            };
        }
    }
}   