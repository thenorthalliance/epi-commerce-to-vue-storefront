using System.Collections.Generic;
using System.Linq;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.Vsf.Core.Exporting;
using EPiServer.Vsf.Core.Mapping;
using EPiServer.Vsf.DataExport.Model;
using EPiServer.Vsf.DataExport.Utils;
using EPiServer.Vsf.DataExport.Utils.Epi;
using Mediachase.Commerce.InventoryService;

namespace EPiServer.Vsf.DataExport.Mapping
{
    public interface IProductMapper<TProduct> : IMapper<ProductContent, TProduct> where TProduct : VsfBaseProduct, new()
    {}

    public abstract class ProductBaseMapper<TProduct> : IProductMapper<TProduct> where TProduct : VsfBaseProduct, new()
    {
        protected readonly IVsfPriceService PriceService;
        protected readonly IContentLoaderWrapper ContentLoaderWrapper;
        protected readonly IInventoryService InventoryService;

        public ProductBaseMapper(IVsfPriceService priceService, IContentLoaderWrapper contentLoaderWrapper, IInventoryService inventoryService)
        {
            PriceService = priceService;
            ContentLoaderWrapper = contentLoaderWrapper;
            InventoryService = inventoryService;
        }

        public abstract TProduct Map(ProductContent source);

        protected virtual TProduct BaseMap(ProductContent source)
        {
            var imageUrl = source.CommerceMediaCollection.FirstOrDefault()?.AssetLink.GetUrl();
            var thumbnail = UrlHelper.GetAsThumbnailUrl(imageUrl);
            var variantQuantity = GetTotalInventoryByEntry(source.Code);
            var configurableOptions = GetProductConfigurableOptions(source).ToList();

            var productVariations = source.GetVariants();

            var productPrice = PriceService.GetDefaultPrice(source.ContentLink);

            var product = new TProduct
            {
                Id = source.ContentLink.ID,
                Name = source.DisplayName,
                UrlKey = source.RouteSegment,
                UrlPath = source.SeoUri,
                IsInStock = new VsfStock {IsInStock = true, Quantity = (int) variantQuantity},
                Sku = source.Code,
                TaxClassId = null,
                MediaGallery = GetGallery(source),
                Image = imageUrl ?? "",
                Thumbnail = thumbnail,
                FinalPrice = productPrice,
                Price = productPrice,
                TypeId = "configurable",
                SpecialPrice = null,
                NewsFromDate = null,
                NewsToDate = null,
                SpecialFromDate = null,
                SpecialToDate = null,
                CategoryIds = source.GetCategories().Select(x => x.ID.ToString()),
                Category = source.GetCategories().Select(x =>
                    new CategoryListItem {Id = x.ID, Name = ContentLoaderWrapper.Get<NodeContent>(x).DisplayName}),
                Status = 1,
                Visibility = source.Status.Equals(VersionStatus.Published) ? 4 : 0,
                Weight = 1,
                HasOptions = configurableOptions.Count > 1 ? "1" : "0",
                RequiredOptions = "0",
                ConfigurableOptions = configurableOptions,
                UpdatedAt = source.Changed,
                CreatedAt = source.Created
            };

            product.ConfigurableChildren = productVariations
                .Select(v => MapVariant(product, ContentLoaderWrapper.Get<VariationContent>(v))).ToList();
            return product;
        }

        protected virtual IEnumerable<Media> GetGallery(ProductContent content)
        {
            if (content == null)
                return null;

            var result = new List<Media>();
            var variants = content.GetVariants();
            
            foreach (var variant in variants)
            {
                
                var imageReference = ContentLoaderWrapper.Get<VariationContent>(variant).CommerceMediaCollection
                    .Select(x => x.AssetLink).FirstOrDefault();
                
                result.Add(new Media
                {
                    Image = imageReference.GetUrl()
                });
            }

            return result;
        }

        protected virtual IEnumerable<ConfigurableOption> GetProductConfigurableOptions(ProductContent product)
        {
            var options = new List<ConfigurableOption>();
            var variants = product.GetVariants();
            
            var index = 0;

            foreach (var variant in variants)
            {
                var variantProperties = ContentLoaderWrapper.GetVariantVsfProperties(variant);

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

        protected virtual ConfigurableOptionValue MapConfigurableOptionValue(PropertyData variantProperty, int order)
        {
            return new ConfigurableOptionValue
            {
                DefaultLabel = variantProperty.Value.ToString(),
                Label = variantProperty.Value.ToString(),
                Order = order,
                ValueIndex = variantProperty.AsAttributeValue()
            };
        }

        protected virtual ConfigurableOption MapConfigurableOption(PropertyData variantProperty, int position, int productId, List<ConfigurableOptionValue> values)
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

        protected virtual ConfigurableChild MapVariant(TProduct product, VariationContent variation) 
        {
            var variantQuantity = GetTotalInventoryByEntry(variation.Code);
            var imageUrl = variation.CommerceMediaCollection.FirstOrDefault()?.AssetLink.GetUrl();
            var thumbnail = UrlHelper.GetAsThumbnailUrl(imageUrl);
            var price = PriceService.GetDefaultPrice(variation.PriceReference);

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
                { "stock", new VsfStock { IsInStock = true, Quantity = (int)variantQuantity }},
                { "name", variation.DisplayName}
            };

            var variantProperties = ContentLoaderWrapper.GetVariantVsfProperties(variation.ContentLink);
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
        protected virtual decimal GetTotalInventoryByEntry(string code)
        {
            return InventoryService.QueryByEntry(new[] { code }).Sum(x => x.PurchaseAvailableQuantity);
        }
    }
}