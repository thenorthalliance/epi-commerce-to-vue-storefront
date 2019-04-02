using System;
using System.Collections.Generic;
using Nest;

namespace DataMigration.Output.Model
{
    public class Product 
    {
        [PropertyName("id")]
        public int Id { get; set; }

        [PropertyName("name")]
        public string Name { get; set; }

        [Keyword(Name = "sku")]
        public string Sku { get; set; }

        [PropertyName("tax_class_id")]
        public string TaxClassId { get; set; }

        [PropertyName("image")]
        public string Image { get; set; }

        [PropertyName("thumbnail")]
        public string Thumbnail { get; set; }

        [PropertyName("media_gallery")]
        public IEnumerable<Media> MediaGallery { get; set; }

        [Keyword(Name="url_key")]
        public string UrlKey { get; set; }

        [Keyword(Name="url_path")]
        public string UrlPath { get; set; }

        [PropertyName("price")]
        public decimal Price { get; set; }

        [PropertyName("final_price")]
        public decimal FinalPrice { get; set; }

        //nullable
        [PropertyName("special_price")]
        public int? SpecialPrice { get; set; }

        [PropertyName("stock")]
        public Stock IsInStock { get; set; }

        [PropertyName("category_ids")]
        public IEnumerable<string> CategoryIds { get; set; }

        [Keyword(Name = "color_options")]
        public IEnumerable<string> ColorOptions { get; set; }

        [Keyword(Name = "size_options")]
        public IEnumerable<string> SizeOptions { get; set; }

        [PropertyName("type_id")]
        public string TypeId { get; set; }

        [PropertyName("has_options")]
        public string HasOptions { get; set; }

        [PropertyName("required_options")]
        public string RequiredOptions { get; set; }

        [PropertyName("status")]
        public int Status { get; set; }

        // 4 -> visible, 0 -> invisible
        [PropertyName("visibility")]
        public int Visibility { get; set; }

        [PropertyName("description")]
        public string Description { get; set; }

        [PropertyName("category")]
        public IEnumerable<CategoryListItem> Category { get; set; }

        
        [PropertyName("weight")]
        public int Weight { get; set; }

        //nullable
        [PropertyName("news_from_date")]
        public DateTime? NewsFromDate { get; set; }       
        
        //nullable
        [PropertyName("updated_at")]
        public DateTime? UpdatedAt { get; set; }
        
        //nullable
        [PropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }

        //nullable
        [PropertyName("news_to_date")]
        public DateTime? NewsToDate { get; set; }

        //nullable
        [PropertyName("special_from_date")]
        public DateTime? SpecialFromDate { get; set; }

        //nullable
        [PropertyName("special_to_date")]
        public DateTime? SpecialToDate { get; set; }

        [PropertyName("configurable_children")]
        public IEnumerable<ConfigurableChild> ConfigurableChildren { get; set; }

        [PropertyName("configurable_options")]
        public IEnumerable<ConfigurableOption> ConfigurableOptions { get; set; }
    }
}
