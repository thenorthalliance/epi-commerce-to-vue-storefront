using System.Collections.Generic;
using System.Linq;
using DataMigration.Input.Episerver.ContentProperty.Model;
using DataMigration.Output.ElasticSearch.Entity.Attribute.Helper;
using Nest;

namespace DataMigration.Output.ElasticSearch.Entity.Attribute.Model
{
    public class Attribute
    {
        public Attribute(EpiContentProperty source)
        {
            Id = source.Id;
            EntityTypeId = 4;
            AttributeModel = null;
            BackendModel = null;
            BackendType = "int";
            BackendTable = null;
            FrontendModel = null;
            FrontendClass = null;
            SourceModel = "eav/entity_attribute_source_table";
            IsRequired = false;
            IsUserDefined = true;
            DefaultValue = "";
            IsUnique = false;
            Note = null;
            AttributeId = source.Id;
            FrontendInputRenderer = null;
            IsGlobal = true;
            IsVisible = true;
            IsSearchable = true;
            IsFilterable = true;
            IsComparable = false;
            IsVisibleOnFront = false;
            IsHtmlAllowedOnFront = true;
            IsUsedForPriceRules = false;
            IsFilterableInSearch = false;
            UsedInProductListing = 0;
            UsedForSortBy = 0;
            IsConfigurable = true;
            ApplyTo = new[] {"simple", "grouped", "configurable"};
            IsVisibleInAdvancedSearch = false;
            Position = 0;
            IsWysiwygEnabled = false;
            IsUsedForPromoRules = false;
            SearchWeight = 1;
            FrontendInput = "select";
            Name = source.Name;
            FrontendLabel = source.Name;
            AttributeCode = source.Name.Replace(" ", "_").ToLower();
            Options = source.Values.Select(x => AttributeHelper.Instance.GetAttributeOption(source.Id, x));
        }

        [PropertyName("id")]
        public int Id { get; set; }

        [PropertyName("name")]
        public string Name { get; set; }

        [PropertyName("position")]
        public int Position { get; set; }

        [PropertyName("entity_type_id")]
        public int EntityTypeId { get; set; }

        [PropertyName("attribute_id")]
        public int AttributeId { get; set; }

        [PropertyName("attribute_code")]
        public string AttributeCode { get; set; }

        [PropertyName("is_required")]
        public bool IsRequired { get; set; }

        [PropertyName("is_user_defined")]
        public bool IsUserDefined { get; set; }

        [PropertyName("is_unique")]
        public bool IsUnique { get; set; }

        [PropertyName("is_global")]
        public bool IsGlobal { get; set; }

        [PropertyName("is_visible")]
        public bool IsVisible { get; set; }

        [PropertyName("is_searchable")]
        public bool IsSearchable { get; set; }

        [PropertyName("is_filterable")]
        public bool IsFilterable { get; set; }

        [PropertyName("is_comparable")]
        public bool IsComparable { get; set; }

        [PropertyName("is_visible_on_front")]
        public bool IsVisibleOnFront { get; set; }

        [PropertyName("is_html_allowed_on_front")]
        public bool IsHtmlAllowedOnFront { get; set; }

        [PropertyName("is_used_for_price_rules")]
        public bool IsUsedForPriceRules { get; set; }

        [PropertyName("is_filterable_in_search")]
        public bool IsFilterableInSearch { get; set; }

        [PropertyName("is_configurable")]
        public bool IsConfigurable { get; set; }

        [PropertyName("is_visible_in_advanced_search")]
        public bool IsVisibleInAdvancedSearch { get; set; }

        [PropertyName("is_wysiwyg_enabled")]
        public bool IsWysiwygEnabled { get; set; }

        [PropertyName("is_used_for_promo_rules")]
        public bool IsUsedForPromoRules { get; set; }

        [PropertyName("frontend_input")]
        public string FrontendInput { get; set; }

        [PropertyName("frontend_label")]
        public string FrontendLabel { get; set; }

        [PropertyName("frontend_class")]
        public string FrontendClass { get; set; }

        [PropertyName("source_model")]
        public string SourceModel { get; set; }

        [PropertyName("default_value")]
        public string DefaultValue { get; set; }

        [PropertyName("note")]
        public string Note { get; set; }

        [PropertyName("apply_to")]
        public IEnumerable<string> ApplyTo { get; set; }

        [PropertyName("search_weight")]
        public int SearchWeight { get; set; }

        [PropertyName("used_for_sort_by")]
        public int UsedForSortBy { get; set; }

        [PropertyName("used_in_product_listing")]
        public int UsedInProductListing { get; set; }

        [PropertyName("frontend_input_renderer")]
        public string FrontendInputRenderer { get; set; }

        [PropertyName("backend_type")]
        public string BackendType { get; set; }

        [PropertyName("attribute_model")]
        public string AttributeModel { get; set; }

        [PropertyName("backend_model")]
        public string BackendModel { get; set; }

        [PropertyName("backend_table")]
        public string BackendTable { get; set; }

        [PropertyName("frontend_model")]
        public string FrontendModel { get; set; }

        [PropertyName("options")]
        public IEnumerable<Option> Options { get; set; }
    }
}