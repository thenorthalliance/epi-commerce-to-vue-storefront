using System.Collections.Generic;
using System.Linq;
using DataMigration.Input.Episerver.Common.Helpers;
using DataMigration.Input.Episerver.Common.Model;
using DataMigration.Input.Episerver.Common.Service;
using DataMigration.Input.Episerver.Product.Model;
using DataMigration.Output.ElasticSearch.Entity.Attribute.Helper;
using DataMigration.Output.ElasticSearch.Entity.Product.Model;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;

namespace DataMigration.Mapper
{
    public class ProductMapper : IMapper<Product>
    {
        public Product Map(CmsObjectBase cmsObject)
        {
            if (!(cmsObject is EpiProduct source))
                return null;

            return NewProduct(source);
        }
        
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

        private static Product NewProduct(EpiProduct epiProduct)
        {
            var epiProductProductContent = epiProduct.ProductContent;
            var imageUrl = epiProductProductContent.CommerceMediaCollection.Select(x => UrlHelper.GetUrl(x.AssetLink)).FirstOrDefault();
            var variantQuantity = InventoryService.GetTotalInventoryByEntry(epiProductProductContent.Code);
            var configurableOptions = GetProductConfigurableOptions(epiProduct.ProductContent).ToList();
            var productVariations = epiProduct.ProductContent.GetVariants();

            var product = new Product
            {
                Id = epiProduct.Id,
                Name = epiProduct.ProductContent.DisplayName,
                UrlKey = epiProductProductContent.RouteSegment.Replace("-", ""),
                UrlPath = epiProductProductContent.SeoUri.Replace("-", ""),
                IsInStock = new Stock {IsInStock = true, Quantity = (int) variantQuantity},
                Sku = epiProductProductContent.Code.Replace("-", ""),
                TaxClassId = null,
                MediaGallery = GetGallery(epiProductProductContent as ProductContent),
                Image = imageUrl ?? "",
                Thumbnail = imageUrl ?? "",
                Price = PriceService.GetPrice(epiProductProductContent.ContentLink),
                Description = epiProduct.ProductContent.GetType().GetProperty("Description") ?.GetValue(epiProduct.ProductContent, null)?.ToString(),
                TypeId = "configurable",
                SpecialPrice = null,
                NewsFromDate = null,
                NewsToDate = null,
                SpecialFromDate = null,
                SpecialToDate = null,
                CategoryIds = epiProduct.ProductContent.GetCategories().Select(x => x.ID.ToString()),
                Category = epiProduct.ProductContent.GetCategories().Select(x => new CategoryListItem {Id = x.ID, Name = ContentHelper.GetContent<NodeContent>(x).DisplayName}),
                Status = 1,
                Visibility = epiProduct.ProductContent.Status.Equals(VersionStatus.Published) ? 4 : 0,
                Weight = 1,
                HasOptions = configurableOptions.Count > 1 ? "1" : "0",
                RequiredOptions = "0",
                ConfigurableOptions = configurableOptions,
                UpdatedAt = epiProduct.ProductContent.Changed
            };

            product.ConfigurableChildren = productVariations.Select(x => MapVariant(product, ContentHelper.GetContent<VariationContent>(x))).ToList();

            //TODO how to make it better, color_options etc are needed to filetering in category view and it is needed to be a number
            foreach (var option in configurableOptions) 
            {
                if (option.Label.Equals("Color"))
                {
                    product.ColorOptions = option.Values.Select(x => x.ValueIndex);
                }

                if (option.Label.Equals("Size"))
                {
                    product.SizeOptions = option.Values.Select(x => x.ValueIndex);
                }
            }

            return product;
        }


        private static IEnumerable<ConfigurableOption> GetProductConfigurableOptions(ProductContent product)
        {
            var options = new List<ConfigurableOption>();
            var variants = product.GetVariants();
            var index = 0;
            foreach (var variant in variants)
            {
                var variantProperties = ContentHelper.GetVariantVsfProperties(variant);

                foreach (var variantProperty in variantProperties)
                {
                    if (variantProperty.Value == null)
                    {
                        continue;
                    }
                    var optionValue = new ConfigurableOptionValue(variantProperty, index);
                    var currentOption = options.FirstOrDefault(x => x.Label.Equals(variantProperty.Name));
                    if (currentOption == null)
                    {
                        var position = options.Count == 0 ? 0 : options.Count + 1;
                        var values = new List<ConfigurableOptionValue>()
                        {
                            optionValue
                        };
                        options.Add(new ConfigurableOption(variantProperty, position, product.ContentLink.ID, values));
                    }
                    else
                    {
                        var isValue = currentOption.Values.FirstOrDefault(x => x.Label == variantProperty.Value.ToString()) != null;
                        if (isValue) continue;
                        optionValue.Order = currentOption.Values.Count + 1;
                        currentOption.Values.Add(optionValue);
                    }

                    index = index + 1;
                }
            }

            return options;
        }

        private static Dictionary<string, object> MapVariant(Product product, VariationContent variation)
        {
            var variantQuantity = InventoryService.GetTotalInventoryByEntry(variation.Code);
            var imageUrl = variation.CommerceMediaCollection.Select(x => UrlHelper.GetUrl(x.AssetLink))
                .FirstOrDefault();
            var price = PriceService.GetPrice(variation.ContentLink);
//            var media_gallery = null;//GetGallery(variation as ProductContent);

            var output = new Dictionary<string, object>
            {
                { "id", variation.ContentLink.ID},
                { "product_id", product.Id},
                { "sku", variation.Code.Replace("-", "")},
                { "tax_class_id", null},
                { "thumbnail", imageUrl},
                { "image", imageUrl},
                { "media_gallery", null},
                { "url_key", variation.RouteSegment.Replace("-", "")},
                { "url_path", variation.SeoUri.Replace("-", "")},
                { "price", price},
                { "stock", new Stock { IsInStock = true, Quantity = (int)variantQuantity }},
                { "name", variation.DisplayName}
            };

            var variantProperties = ContentHelper.GetVariantVsfProperties(variation.ContentLink);
            foreach (var variantProperty in variantProperties.Where(p => p.Value != null))
            {
                var name = variantProperty.Name.ToLower();
                var value = AttributeHelper.Instance.GetAttributeOption(variantProperty.PropertyDefinitionID, variantProperty.Value.ToString()).Value;

                if (!output.ContainsKey(name))
                    output.Add(name, value);
                else
                    output[name] = value;
            }

            return output;
        }
    }
}