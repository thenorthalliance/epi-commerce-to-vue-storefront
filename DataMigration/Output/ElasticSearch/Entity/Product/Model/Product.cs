using System;
using System.Collections.Generic;
using System.Linq;
using DataMigration.Input.Episerver.Common.Helpers;
using DataMigration.Input.Episerver.Product.Model;
using DataMigration.Output.ElasticSearch.Entity.Attribute.Helper;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DataMigration.Output.ElasticSearch.Entity.Product.Model
{
    public class Product : ProductBase
    {
        public Product(EpiProduct epiProduct) : base(epiProduct.ProductContent)
        {
            var configurableOptions = GetProductConfigurableOptions(epiProduct.ProductContent).ToArray();
            var productVariations = epiProduct.ProductContent.GetVariants();

            Id = epiProduct.Id;
            Description = epiProduct.ProductContent.GetType().GetProperty("Description")
                ?.GetValue(epiProduct.ProductContent, null)?.ToString();
            Name = epiProduct.ProductContent.DisplayName;
            TypeId = "configurable";
            SpecialPrice = null;
            NewsFromDate = null;
            NewsToDate = null;
            SpecialFromDate = null;
            SpecialToDate = null;
            CategoryIds = epiProduct.ProductContent.GetCategories().Select(x => x.ID.ToString());
            Category = epiProduct.ProductContent.GetCategories().Select(x =>
                new CategoryListItem {Id = x.ID, Name = ContentHelper.GetContent<NodeContent>(x).DisplayName});
            Status = 1;
            Visibility = epiProduct.ProductContent.Status.Equals(VersionStatus.Published) ? 4 : 0;
            Weight = 1;
            ConfigurableChildren = productVariations.Select(x => MapVariant(ContentHelper.GetContent<VariationContent>(x), epiProduct.Id)).ToArray();
            HasOptions = configurableOptions.Length > 1 ? "1" : "0";
            RequiredOptions = "0";
            ConfigurableOptions = configurableOptions;
        }

        private static IEnumerable<ConfigurableOption> GetProductConfigurableOptions(ProductContent product)
        {
            var options = new List<ConfigurableOption>();
            var variants = product.GetVariants();
            foreach (var variant in variants)
            {
                var variantProperties = ContentHelper.GetVariantVsfProperties(variant);
                foreach (var variantProperty in variantProperties)
                {
                    if (variantProperty.Value == null)
                    {
                        continue;
                    }
                    var optionValue = new ConfigurableOptionValue
                    {
                        DefaultLabel = variantProperty.Value.ToString(),
                        Label = variantProperty.Value.ToString(),
                        Order = 0,
                        ValueIndex = AttributeHelper.CreateValueIndex(variantProperty.PropertyDefinitionID, variantProperty.Value.ToString())
                    };
                    var currentOption = options.FirstOrDefault(x => x.Label.Equals(variantProperty.Name));
                    if (currentOption == null)
                    {
                        options.Add(new ConfigurableOption
                        {
                            Id = variantProperty.PropertyDefinitionID,
                            Position = options.Count == 0 ? 0 : options.Count + 1,
                            Label = variantProperty.Name,
                            AttributeCode = "prodopt-" + variantProperty.Name.Replace(" ", "_").ToLower(),
                            FrontentLabel = variantProperty.Name,
                            ProductId = product.ContentLink.ID,
                            Values = new List<ConfigurableOptionValue>()
                            {
                                optionValue
                            }
                        });
                    }
                    else
                    {
                        var isValue = currentOption.Values.FirstOrDefault(x => x.Label == variantProperty.Value.ToString()) != null;
                        if (isValue) continue;
                        optionValue.Order = currentOption.Values.Count + 1;
                        currentOption.Values.Add(optionValue);
                    }
                }
            }

            return options;
        }

        private static JObject MapVariant(VariationContent variation, int productId)
        {
            var variant = new Variant(variation, productId);
            var resultVariantWithOptions = JObject.FromObject(variant);
            var variantProperties = ContentHelper.GetVariantVsfProperties(variation.ContentLink);
            foreach (var variantProperty in variantProperties)
            {
                if (variantProperty.Value == null)
                {
                    continue;
                }
                resultVariantWithOptions.Add(new JProperty(variantProperty.Name, variantProperty.Value.ToString()));
            }
            return resultVariantWithOptions;
        }

        [JsonProperty("category_ids")]
        public IEnumerable<string> CategoryIds { get; set; }

        [JsonProperty("type_id")]
        public string TypeId { get; set; }

        [JsonProperty("has_options")]
        public string HasOptions { get; set; }

        [JsonProperty("required_options")]
        public string RequiredOptions { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        // 4 -> visible, 0 -> invisible
        [JsonProperty("visibility")]
        public int Visibility { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        //nullable
        [JsonProperty("special_price")]
        public int? SpecialPrice { get; set; }

        [JsonProperty("category")]
        public IEnumerable<CategoryListItem> Category { get; set; }

        [JsonProperty("weight")]
        public int Weight { get; set; }

        //nullable
        [JsonProperty("news_from_date")]
        public DateTime? NewsFromDate { get; set; }

        //nullable
        [JsonProperty("news_to_date")]
        public DateTime? NewsToDate { get; set; }

        //nullable
        [JsonProperty("special_from_date")]
        public DateTime? SpecialFromDate { get; set; }

        //nullable
        [JsonProperty("special_to_date")]
        public DateTime? SpecialToDate { get; set; }

        [JsonProperty("configurable_children")]
        public JObject[] ConfigurableChildren { get; set; }

        [JsonProperty("configurable_options")]
        public ConfigurableOption[] ConfigurableOptions { get; set; }
    }
}
