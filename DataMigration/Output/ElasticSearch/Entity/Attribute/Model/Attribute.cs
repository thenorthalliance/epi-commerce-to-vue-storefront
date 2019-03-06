using System.Collections.Generic;
using System.Linq;
using DataMigration.Input.Episerver.ContentProperty.Model;
using DataMigration.Output.ElasticSearch.Entity.Attribute.Helper;
using Newtonsoft.Json;

namespace DataMigration.Output.ElasticSearch.Entity.Attribute.Model
{
    public class Attribute : Entity
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

        [JsonProperty("position")]
        public int Position { get; set; }

        [JsonProperty("entity_type_id")]
        public int EntityTypeId { get; set; }

        [JsonProperty("attribute_id")]
        public int AttributeId { get; set; }

        [JsonProperty("attribute_code")]
        public string AttributeCode { get; set; }

        [JsonProperty("is_required")]
        public bool IsRequired { get; set; }

        [JsonProperty("is_user_defined")]
        public bool IsUserDefined { get; set; }

        [JsonProperty("is_unique")]
        public bool IsUnique { get; set; }

        [JsonProperty("is_global")]
        public bool IsGlobal { get; set; }

        [JsonProperty("is_visible")]
        public bool IsVisible { get; set; }

        [JsonProperty("is_searchable")]
        public bool IsSearchable { get; set; }

        [JsonProperty("is_filterable")]
        public bool IsFilterable { get; set; }

        [JsonProperty("is_comparable")]
        public bool IsComparable { get; set; }

        [JsonProperty("is_visible_on_front")]
        public bool IsVisibleOnFront { get; set; }

        [JsonProperty("is_html_allowed_on_front")]
        public bool IsHtmlAllowedOnFront { get; set; }

        [JsonProperty("is_used_for_price_rules")]
        public bool IsUsedForPriceRules { get; set; }

        [JsonProperty("is_filterable_in_search")]
        public bool IsFilterableInSearch { get; set; }

        [JsonProperty("is_configurable")]
        public bool IsConfigurable { get; set; }

        [JsonProperty("is_visible_in_advanced_search")]
        public bool IsVisibleInAdvancedSearch { get; set; }

        [JsonProperty("is_wysiwyg_enabled")]
        public bool IsWysiwygEnabled { get; set; }

        [JsonProperty("is_used_for_promo_rules")]
        public bool IsUsedForPromoRules { get; set; }

        [JsonProperty("frontend_input")]
        public string FrontendInput { get; set; }

        [JsonProperty("frontend_label")]
        public string FrontendLabel { get; set; }

        [JsonProperty("frontend_class")]
        public string FrontendClass { get; set; }

        [JsonProperty("source_model")]
        public string SourceModel { get; set; }

        [JsonProperty("default_value")]
        public string DefaultValue { get; set; }

        [JsonProperty("note")]
        public string Note { get; set; }

        [JsonProperty("apply_to")]
        public IEnumerable<string> ApplyTo { get; set; }

        [JsonProperty("search_weight")]
        public int SearchWeight { get; set; }

        [JsonProperty("used_for_sort_by")]
        public int UsedForSortBy { get; set; }

        [JsonProperty("used_in_product_listing")]
        public int UsedInProductListing { get; set; }

        [JsonProperty("frontend_input_renderer")]
        public string FrontendInputRenderer { get; set; }

        [JsonProperty("backend_type")]
        public string BackendType { get; set; }

        [JsonProperty("attribute_model")]
        public string AttributeModel { get; set; }

        [JsonProperty("backend_model")]
        public string BackendModel { get; set; }

        [JsonProperty("backend_table")]
        public string BackendTable { get; set; }

        [JsonProperty("frontend_model")]
        public string FrontendModel { get; set; }

        [JsonProperty("options")]
        public IEnumerable<Option> Options { get; set; }
    }
}