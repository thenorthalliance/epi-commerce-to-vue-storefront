using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DataMigration.Output.ElasticSearch.Entity.Product.Model
{
    public class Product : Entity
    {
        [JsonProperty("category_ids")]
        public IEnumerable<string> CategoryIds { get; set; }

        //simple is default
        [JsonProperty("type_id")]
        public string TypeId { get; set; }

        [JsonProperty("sku")]
        public string Sku { get; set; }

        [JsonProperty("has_options")]
        public string HasOptions { get; set; }

        [JsonProperty("required_options")]
        public string RequiredOptions { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        // 4 -> visible, 0 -> invisible
        [JsonProperty("visibility")]
        public int Visibility { get; set; }

        [JsonProperty("tax_class_id")]
        public string TaxClassId { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
        
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

        //nullable
        [JsonProperty("special_price")]
        public int SpecialPrice { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("stock")]
        public Stock IsInStock { get; set; }

        [JsonProperty("category")]
        public IEnumerable<CategoryListItem> Category { get; set; }

        [JsonProperty("weight")]
        public int Weight { get; set; }

        //nullable
        [JsonProperty("news_from_date")]
        public DateTime NewsFromDate { get; set; }

        //nullable
        [JsonProperty("news_to_date")]
        public DateTime NewsToDate { get; set; }

        //nullable
        [JsonProperty("special_from_date")]
        public DateTime SpecialFromDate { get; set; }

        //nullable
        [JsonProperty("special_to_date")]
        public DateTime SpecialToDate { get; set; }
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
}
