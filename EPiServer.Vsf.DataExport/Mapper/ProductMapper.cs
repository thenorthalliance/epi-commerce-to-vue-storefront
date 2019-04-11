using System.Collections.Generic;
using System.Linq;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.Vsf.DataExport.Helpers;
using EPiServer.Vsf.DataExport.Model.Elastic;
using EPiServer.Vsf.DataExport.Utils.Epi;

namespace EPiServer.Vsf.DataExport.Mapper
{
    public class ProductMapper : IMapper<ProductContent, Product>
    {
        private readonly PriceService _priceService;
        private readonly InventoryService _inventoryService;
        private readonly ContentService _contentService;

        public ProductMapper(PriceService priceService, InventoryService inventoryService, ContentService contentService)
        {
            _priceService = priceService;
            _inventoryService = inventoryService;
            _contentService = contentService;
        }

        public Product Map(ProductContent source)
        {

            var imageUrl = source.CommerceMediaCollection.FirstOrDefault()?.AssetLink.GetUrl();
            var thumbnail = UrlHelper.GetAsThumbnailUrl(imageUrl);
            var variantQuantity = _inventoryService.GetTotalInventoryByEntry(source.Code);
            var configurableOptions = GetProductConfigurableOptions(source).ToList();
            var productVariations = source.GetVariants();
            var productPrice = _priceService.GetDefaultPrice(source.Code);

            var product = new Product
            {
                Id = source.ContentLink.ID,
                Name = source.DisplayName,
                UrlKey = source.RouteSegment,
                UrlPath = source.SeoUri,
                IsInStock = new Stock {IsInStock = true, Quantity = (int) variantQuantity},
                Sku = source.Code,
                TaxClassId = null,
                MediaGallery = GetGallery(source),
                Image = imageUrl ?? "",
                Thumbnail = thumbnail,
                FinalPrice = productPrice,
                Price = productPrice,
                Description = source.GetType().GetProperty("Description") ?.GetValue(source, null)?.ToString(),
                TypeId = "configurable",
                SpecialPrice = null,
                NewsFromDate = null,
                NewsToDate = null,
                SpecialFromDate = null,
                SpecialToDate = null,
                CategoryIds = source.GetCategories().Select(x => x.ID.ToString()),
                Category = source.GetCategories().Select(x => new CategoryListItem {Id = x.ID, Name = _contentService.GetContent<NodeContent>(x).DisplayName}),
                Status = 1,
                Visibility = source.Status.Equals(VersionStatus.Published) ? 4 : 0,
                Weight = 1,
                HasOptions = configurableOptions.Count > 1 ? "1" : "0",
                RequiredOptions = "0",
                ConfigurableOptions = configurableOptions,
                UpdatedAt = source.Changed,
                CreatedAt = source.Created
            };

            product.ConfigurableChildren = productVariations.Select(v => MapVariant(product, _contentService.GetContent<VariationContent>(v))).ToList();

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
        
        private IEnumerable<Media> GetGallery(ProductContent content)
        {
            if (content == null)
                return null;

            var result = new List<Media>();
            var variants = content.GetVariants();
            foreach (var variant in variants)
            {
                var imageReference = _contentService.GetContent<VariationContent>(variant).CommerceMediaCollection
                    .Select(x => x.AssetLink).FirstOrDefault();
                result.Add(new Media
                {
                    Image = imageReference.GetUrl()
                });
            }

            return result;
        }

        //TODO we should have a separated class for this - probably...
        private IEnumerable<ConfigurableOption> GetProductConfigurableOptions(ProductContent product)
        {
            var options = new List<ConfigurableOption>();
            var variants = product.GetVariants();
            var index = 0;
            foreach (var variant in variants)
            {
                var variantProperties = _contentService.GetVariantVsfProperties(variant);

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

        private ConfigurableChild MapVariant(Product product, VariationContent variation)
        {
            var variantQuantity = _inventoryService.GetTotalInventoryByEntry(variation.Code);
            var imageUrl = variation.CommerceMediaCollection.FirstOrDefault()?.AssetLink.GetUrl();
            var thumbnail = UrlHelper.GetAsThumbnailUrl(imageUrl);
            var price = _priceService.GetDefaultPrice(variation.Code);
//            GetGallery(variation as ProductContent);

            var output = new ConfigurableChild
            {
                { "id", variation.ContentLink.ID},
                { "product_id", product.Id},
                { "sku", variation.Code},
                { "tax_class_id", null},
                { "thumbnail", thumbnail},
                { "image", imageUrl},
                { "media_gallery", null},
                { "url_key", variation.RouteSegment},
                { "url_path", variation.SeoUri},
                { "price", price},
                { "stock", new Stock { IsInStock = true, Quantity = (int)variantQuantity }},
                { "name", variation.DisplayName}
            };

            var variantProperties = _contentService.GetVariantVsfProperties(variation.ContentLink);
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