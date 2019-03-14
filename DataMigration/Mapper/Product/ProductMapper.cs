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

namespace DataMigration.Mapper.Product
{
    public class ProductMapper : IMapper<Output.ElasticSearch.Entity.Product.Model.Product>
    {
        public Output.ElasticSearch.Entity.Product.Model.Product Map(CmsObjectBase cmsObject)
        {
            if (!(cmsObject is EpiProduct source))
                return null;

            var epiProductProductContent = source.ProductContent;
            var imageUrl = epiProductProductContent.CommerceMediaCollection.FirstOrDefault()?.AssetLink.GetUrl();
            var thumbnail = UrlHelper.GetAsThumbnailUrl(imageUrl);
            var variantQuantity = InventoryService.GetTotalInventoryByEntry(epiProductProductContent.Code);
            var configurableOptions = GetProductConfigurableOptions(source.ProductContent).ToList();
            var productVariations = source.ProductContent.GetVariants();
            var productPrice = PriceService.GetPrice(epiProductProductContent.ContentLink);

            var product = new Output.ElasticSearch.Entity.Product.Model.Product
            {
                Id = source.Id,
                Name = source.ProductContent.DisplayName,
                UrlKey = epiProductProductContent.RouteSegment.Replace("-", ""),
                UrlPath = epiProductProductContent.SeoUri.Replace("-", ""),
                IsInStock = new Stock {IsInStock = true, Quantity = (int) variantQuantity},
                Sku = epiProductProductContent.Code.Replace("-", ""),
                TaxClassId = null,
                MediaGallery = GetGallery(epiProductProductContent),
                Image = imageUrl ?? "",
                Thumbnail = thumbnail,
                FinalPrice = productPrice,
                Price = productPrice,
                Description = source.ProductContent.GetType().GetProperty("Description") ?.GetValue(source.ProductContent, null)?.ToString(),
                TypeId = "configurable",
                SpecialPrice = null,
                NewsFromDate = null,
                NewsToDate = null,
                SpecialFromDate = null,
                SpecialToDate = null,
                CategoryIds = source.ProductContent.GetCategories().Select(x => x.ID.ToString()),
                Category = source.ProductContent.GetCategories().Select(x => new CategoryListItem {Id = x.ID, Name = ContentHelper.GetContent<NodeContent>(x).DisplayName}),
                Status = 1,
                Visibility = source.ProductContent.Status.Equals(VersionStatus.Published) ? 4 : 0,
                Weight = 1,
                HasOptions = configurableOptions.Count > 1 ? "1" : "0",
                RequiredOptions = "0",
                ConfigurableOptions = configurableOptions,
                UpdatedAt = source.ProductContent.Changed
            };

            product.ConfigurableChildren = productVariations.Select(v => MapVariant(product, ContentHelper.GetContent<VariationContent>(v))).ToList();

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
        
        private static IEnumerable<Media> GetGallery(ProductContent content)
        {
            if (content == null)
                return null;

            var result = new List<Media>();
            var variants = content.GetVariants();
            foreach (var variant in variants)
            {
                var imageReference = ContentHelper.GetContent<VariationContent>(variant).CommerceMediaCollection
                    .Select(x => x.AssetLink).FirstOrDefault();
                result.Add(new Media
                {
                    Image = imageReference.GetUrl()
                });
            }

            return result;
        }

        //TODO we should have a separated class for this - probably...
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
                        continue;

                    var optionValue = MapConfigurableOptionValue(variantProperty, order: index);
                    var currentOption = options.FirstOrDefault(x => x.Label.Equals(variantProperty.Name));
                    if (currentOption == null)
                    {
                        var position = options.Count;
                        var values = new List<ConfigurableOptionValue>
                        {
                            optionValue
                        };
                        options.Add(MapConfigurableOption(variantProperty, position, product.ContentLink.ID, values));
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

        private static ConfigurableOptionValue MapConfigurableOptionValue(PropertyData variantProperty, int order)
        {
            return new ConfigurableOptionValue
            {
                DefaultLabel = variantProperty.Value.ToString(),
                Label = variantProperty.Value.ToString(),
                Order = order,
                ValueIndex = variantProperty.AsAttributeValue()
            };
        }

        private static ConfigurableOption MapConfigurableOption(PropertyData variantProperty, int position, int productId, List<ConfigurableOptionValue> values)
        {
            return new ConfigurableOption
            {
                Id = variantProperty.PropertyDefinitionID,
                Position = position,
                Label = variantProperty.Name,
                AttributeCode = variantProperty.Name.ToLower().Replace(" ", "_"),
                FrontentLabel = variantProperty.Name.ToLower(),
                ProductId = productId,
                Values = values
            };
        }

        private static ConfigurableChild MapVariant(Output.ElasticSearch.Entity.Product.Model.Product product, VariationContent variation)
        {
            var variantQuantity = InventoryService.GetTotalInventoryByEntry(variation.Code);
            var imageUrl = variation.CommerceMediaCollection.FirstOrDefault()?.AssetLink.GetUrl();
            var thumbnail = UrlHelper.GetAsThumbnailUrl(imageUrl);
            var price = PriceService.GetPrice(variation.ContentLink);
//            GetGallery(variation as ProductContent);

            var output = new ConfigurableChild
            {
                { "id", variation.ContentLink.ID},
                { "product_id", product.Id},
                { "sku", variation.Code.Replace("-", "")}, //TODO this repeats a lot !
                { "tax_class_id", null},
                { "thumbnail", thumbnail},
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
                var value = variantProperty.AsAttributeValue();

                if (!output.ContainsKey(name))
                    output.Add(name, value);
                else
                    output[name] = value;
            }

            return output;
        }
    }
}