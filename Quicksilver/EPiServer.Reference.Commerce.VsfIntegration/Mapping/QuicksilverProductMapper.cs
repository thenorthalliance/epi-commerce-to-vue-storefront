using System;
using System.Linq;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Reference.Commerce.Shared.Models.Products;
using EPiServer.Vsf.Core.Exporting;
using EPiServer.Vsf.DataExport.Mapping;
using EPiServer.Vsf.DataExport.Utils.Epi;
using Mediachase.Commerce.InventoryService;

namespace EPiServer.Reference.Commerce.VsfIntegration.Mapping
{
    
    public class QuicksilverProductMapper : ProductBaseMapper<QuicksilverVsfProduct>
    {
        public QuicksilverProductMapper(IVsfPriceService priceService, IContentLoaderWrapper contentLoaderWrapper, IInventoryService inventoryService) :
            base(priceService, contentLoaderWrapper, inventoryService)
        {}

        public override QuicksilverVsfProduct Map(ProductContent source)
        {
            if (source.GetOriginalType() == typeof(FashionProduct))
            {
                var fashionContent = (FashionProduct) source;
                var product = BaseMap(source);
                
                product.Description = fashionContent.Description.ToString();

                var configurableOptions = product.ConfigurableOptions;
                product.ColorOptions = configurableOptions.FirstOrDefault(o => o.Label == "Color")?.Values.Select(x => x.ValueIndex);
                product.SizeOptions = configurableOptions.FirstOrDefault(o => o.Label == "Size")?.Values.Select(x => x.ValueIndex);
                
                return product;
            }

            throw new ArgumentException("Source not supported");
        }
    }
}