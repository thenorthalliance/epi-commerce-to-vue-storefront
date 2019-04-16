using System;
using System.Linq;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Reference.Commerce.Shared.Models.Products;
using EPiServer.Vsf.DataExport.Mapping;
using EPiServer.Vsf.DataExport.Utils.Epi;

namespace EPiServer.Reference.Commerce.VsfIntegration.Mapping
{
    public class FasionProductMapper : ProductBaseMapper<FasionVsfProduct>
    {
        public FasionProductMapper(PriceService priceService, InventoryService inventoryService, IContentLoader contentLoader) : base(priceService, inventoryService, contentLoader)
        {}

        public override FasionVsfProduct Map(ProductContent source)
        {
            if (source.GetOriginalType() == typeof(FashionProduct))
            {
                var fasionContent = (FashionProduct) source;
                var product = BaseMap(source);
                product.Description = fasionContent.Description.ToString();

                var configurableOptions = product.ConfigurableOptions;
                product.ColorOptions = configurableOptions.FirstOrDefault(o => o.Label == "Color")?.Values.Select(x => x.ValueIndex);
                product.SizeOptions = configurableOptions.FirstOrDefault(o => o.Label == "Size")?.Values.Select(x => x.ValueIndex);
                
                return product;
            }

            throw new ArgumentException("Source not supported");
        }
    }
}