using System.Collections.Generic;
using System.Linq;
using DataMigration.Helpers;
using DataMigration.Input.Episerver.Common.Model;
using DataMigration.Input.Episerver.Product.Model;
using DataMigration.Output.ElasticSearch.Entity;
using DataMigration.Output.ElasticSearch.Entity.Product.Model;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using Newtonsoft.Json.Linq;

namespace DataMigration.Mapper.Product
{
    public class ProductMapper : IMapper
    {
        public Entity Map(CmsObjectBase cmsObject)
        {
            if (!(cmsObject is EpiProduct source))
            {
                return null;
            }

            var configurableOptions = GetProductConfigurableOptions(source.ProductContent).ToArray();
            var productVariations = ContentHelper.GetVariants(source.ProductContent.ContentLink);
            var productBase = MapProductBase(source.ProductContent);

            return new Output.ElasticSearch.Entity.Product.Model.Product(productBase)
            {
                Id = source.Id,
                Description = source.ProductContent.GetType().GetProperty("Description")?.GetValue(source.ProductContent, null)?.ToString(),
                Name = source.ProductContent.DisplayName,
                TypeId = "configurable",
                SpecialPrice = null,
                NewsFromDate = null,
                NewsToDate = null,
                SpecialFromDate = null,
                SpecialToDate = null,
                CategoryIds = source.ProductContent.GetCategories().Select(x => x.ID.ToString()),
                Category = source.ProductContent.GetCategories().Select(x => new CategoryListItem { Id = x.ID, Name = ContentHelper.GetContent<NodeContent>(x).DisplayName }),
                Status = 1,
                Visibility = source.ProductContent.Status.Equals(VersionStatus.Published) ? 4 : 0,
                Weight = 1,
                ConfigurableChildren = productVariations.Select(x => MapVariant(x, source.Id)).ToArray(),
                HasOptions = configurableOptions.Length > 1 ? "1" : "0",
                RequiredOptions = "0",
                ConfigurableOptions = configurableOptions
            };
        }

        private static IEnumerable<Option> GetProductConfigurableOptions(ProductContent product)
        {
            var options = new List<Option>();
            var variants = ContentHelper.GetVariants(product.ContentLink);
            foreach (var variant in variants)
            {
                var variantOptions = ContentHelper.GetVariantOptions(variant.ContentLink);
                foreach (var variantOption in variantOptions)
                {
                    var optionValue = new OptionValue
                    {
                        DefaultLabel = variantOption.Value,
                        Label = variantOption.Value,
                        Order = 0,
                        ValueData = null,
                        ValueIndex = 0 
                    };
                    var currentOption = options.FirstOrDefault(x => x.Label.Equals(variantOption.Key));
                    if (currentOption == null)
                    {
                        options.Add(new Option
                        {
                            Id = 0, //TODO set when attibutes will be ready
                            Position = options.Count == 0 ? 0 : options.Count + 1,
                            Label = variantOption.Key, 
                            AttributeCode = "prodopt-" + variantOption.Key, //TODO set to id when attributes will be ready
                            FrontentLabel = variantOption.Key,
                            ProductId = product.ContentLink.ID,
                            Values = new List<OptionValue>()
                            {
                                optionValue
                            }
                        });
                    }
                    else
                    {
                        var isValue = currentOption.Values.FirstOrDefault(x => x.Label == variantOption.Value) != null;
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
            var productBase = MapProductBase(variation);
            var variant = new Variant(productBase)
            {
                ProductId = productId
            };
            var resultVariantWithOptions = JObject.FromObject(variant);
            var options = ContentHelper.GetVariantOptions(variation.ContentLink);
            foreach (var option in options)
            {
                resultVariantWithOptions.Add(new JProperty(option.Key, option.Value));
            }
            return resultVariantWithOptions;
        }

        private static ProductBase MapProductBase(EntryContentBase contentBase)
        {
            var imageUrl = contentBase.CommerceMediaCollection.Select(x => UrlHelper.GetUrl(x.AssetLink))
                .FirstOrDefault();
            var variantQuantity = InventoryHelper.GetTotalInventoryByEntry(contentBase.Code);

            var productBase = new ProductBase
            {
                Name = contentBase.DisplayName,
                Id = contentBase.ContentLink.ID,
                UrlKey = contentBase.RouteSegment,
                UrlPath = contentBase.SeoUri,
                IsInStock = new Stock { IsInStock = variantQuantity > 0, Quantity = (int)variantQuantity },
                Sku = contentBase.Code,
                TaxClassId = null, //TODO We don't have taxes yet,
                MediaGallery = contentBase.CommerceMediaCollection.Select(x => new Media { Image = UrlHelper.GetUrl(x.AssetLink) }),
                Image = imageUrl ?? "",
                Thumbnail = imageUrl ?? "",
                Price = PriceHelper.GetPrice(contentBase.ContentLink)
            };

            return productBase;
        }
    }
}