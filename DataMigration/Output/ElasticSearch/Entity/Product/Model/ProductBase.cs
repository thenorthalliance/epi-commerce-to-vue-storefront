using System.Collections.Generic;
using System.Linq;
using DataMigration.Input.Episerver.Common.Helpers;
using DataMigration.Input.Episerver.Common.Service;
using EPiServer.Commerce.Catalog.ContentTypes;
using Nest;

namespace DataMigration.Output.ElasticSearch.Entity.Product.Model
{
    public class ProductBase
    {
        public ProductBase(EntryContentBase contentBase)
        {
            var imageUrl = contentBase.CommerceMediaCollection.Select(x => UrlHelper.GetUrl(x.AssetLink))
                .FirstOrDefault();
            var variantQuantity = InventoryService.GetTotalInventoryByEntry(contentBase.Code);

            Name = contentBase.DisplayName;
            Id = contentBase.ContentLink.ID;
            UrlKey = contentBase.RouteSegment.Replace("-","");
            UrlPath = contentBase.SeoUri.Replace("-", "");
            IsInStock = new Stock {IsInStock = true, Quantity = (int) variantQuantity};
            Sku = contentBase.Code.Replace("-", "");
            TaxClassId = null; //TODO We don't have taxes yet,
            MediaGallery =
                GetGallery(contentBase as ProductContent);
            Image = imageUrl ?? "";
            Thumbnail = imageUrl ?? "";
            Price = PriceService.GetPrice(contentBase.ContentLink);
        }

        [PropertyName("id")]
        public int Id { get; set; }

        [PropertyName("name")]
        public string Name { get; set; }

        [Keyword(Name="sku")]
        public string Sku { get; set; }

        [PropertyName("tax_class_id")]
        public string TaxClassId { get; set; }

        [PropertyName("image")]
        public string Image { get; set; }

        [PropertyName("thumbnail")]
        public string Thumbnail { get; set; }

        [PropertyName("media_gallery")]
        public IEnumerable<Media> MediaGallery { get; set; }

        [PropertyName("url_key")]
        public string UrlKey { get; set; }

        [PropertyName("url_path")]
        public string UrlPath { get; set; }

        [PropertyName("price")]
        public int Price { get; set; }

        [PropertyName("stock")]
        public Stock IsInStock { get; set; }

        private static IEnumerable<Media> GetGallery(ProductContent content)
        {
            if (content == null)
            {
                return null;
            }
            var result = new List<Media>();
            var variants = content.GetVariants();
            foreach (var variant in variants)
            {
                var imageReference = ContentHelper.GetContent<VariationContent>(variant).CommerceMediaCollection
                    .Select(x => x.AssetLink).FirstOrDefault();
                result.Add(new Media
                {
                    Image = UrlHelper.GetUrl(imageReference)
                });
            }

            return result;
        }
    }
}