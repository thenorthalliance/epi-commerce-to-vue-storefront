using System.Linq;
using EPiServer.Vsf.Core.Mapping;
using EPiServer.Vsf.DataExport.Model;
using EPiServer.Vsf.DataExport.Utils;

namespace EPiServer.Vsf.DataExport.Mapping
{
    public interface IAttributeMapper : IMapper<EpiContentProperty, VsfAttribute>
    { }

    public class AttributeMapper : IAttributeMapper
    {
        public VsfAttribute Map(EpiContentProperty source)
        {
            return new VsfAttribute
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