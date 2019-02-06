using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DataMigration.Output.ElasticSearch.Entity.Product.Model
{
    public class ProductBase : Entity
    {
        public ProductBase()
        {
        }

        public ProductBase(ProductBase productBase)
        {
            Name = productBase.Name;
            Id = productBase.Id;
            Sku = productBase.Sku;
            UrlKey = productBase.UrlKey;
            UrlPath = productBase.UrlPath;
            Price = productBase.Price;
            IsInStock = productBase.IsInStock;
            MediaGallery = productBase.MediaGallery;
            Thumbnail = productBase.Thumbnail;
            Image = productBase.Image;
            TaxClassId = productBase.TaxClassId;
        }

        [JsonProperty("sku")]
        public string Sku { get; set; }

        [JsonProperty("tax_class_id")]
        public string TaxClassId { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("thumbnail")]
        public string Thumbnail { get; set; }

        [JsonProperty("media_gallery")]
        public IEnumerable<Media> MediaGallery { get; set; }

        [JsonProperty("url_key")]
        public string UrlKey { get; set; }

        [JsonProperty("url_path")]
        public string UrlPath { get; set; }

        [JsonProperty("price")]
        public int Price { get; set; }

        [JsonProperty("stock")]
        public Stock IsInStock { get; set; }
    }
    public class Product : ProductBase
    {
        public Product(ProductBase productBase) : base(productBase)
        {
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

    public class Media
    {
        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("pos")]
        public int Position { get; set; }

        [JsonProperty("typ")]
        public string Type { get; set; }

        [JsonProperty("lab")]
        public string Label { get; set; }
    }

    public class Stock
    {
        [JsonProperty("is_in_stock")]
        public bool IsInStock { get; set; }

        [JsonProperty("qty")]
        public int Quantity { get; set; }
    }

    public class CategoryListItem
    {
        [JsonProperty("category_id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class ConfigurableOption
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("attribute_code")]
        public string AttributeCode { get; set; }

        [JsonProperty("product_id")]
        public int ProductId { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("position")]
        public int Position { get; set; }

        [JsonProperty("frontend_label")]
        public string FrontentLabel { get; set; }

        [JsonProperty("values")]
        public List<ConfigurableOptionValue> Values { get; set; }
    }

    public class ConfigurableOptionValue
    {
        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("default_label")]
        public string DefaultLabel { get; set; }

        [JsonProperty("order")]
        public int Order { get; set; }

        [JsonProperty("value_index")]
        public string ValueIndex { get; set; }

        [JsonProperty("value_data")]
        public string ValueData { get; set; }
    }

    public class Variant: ProductBase
    {
        public Variant(ProductBase productBase) : base(productBase)
        {
        }

        [JsonProperty("product_id")]
        public int ProductId { get; set; }
    }
}
