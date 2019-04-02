using System.Linq;
using DataMigration.Input.Episerver.ContentProperty.Model;
using DataMigration.Output.ElasticSearch.Entity.Attribute.Helper;

namespace DataMigration.Mapper.Attribute
{
    public class AttributeMapper : IMapper<EpiContentProperty, Output.ElasticSearch.Entity.Attribute.Model.Attribute>
    {
        public Output.ElasticSearch.Entity.Attribute.Model.Attribute Map(EpiContentProperty source)
        {
            return new Output.ElasticSearch.Entity.Attribute.Model.Attribute
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
                ApplyTo = new[] {"simple", "grouped", "configurable"},
                IsVisibleInAdvancedSearch = false,
                Position = 0,
                IsWysiwygEnabled = false,
                IsUsedForPromoRules = false,
                SearchWeight = 1,
                FrontendInput = "select",
                Name = source.Name,
                FrontendLabel = source.Name,
                AttributeCode = source.Name.Replace(" ", "_").ToLower(),
                Options = source.Values.Select(x => AttributeHelper.GetAttributeOption(source.Id, x))
            };
        }
    }
}   