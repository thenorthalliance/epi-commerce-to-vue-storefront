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
    public class SimpleProductMapper : IMapper<VariationContent, VsfSimpleProduct>
    {
        private readonly IVsfPriceService _priceService;
        private readonly IContentLoaderWrapper _contentLoaderWrapper;
        private readonly IInventoryService _inventoryService;

        public SimpleProductMapper(IVsfPriceService priceService, IContentLoaderWrapper contentLoaderWrapper, IInventoryService inventoryService)
        {
            _priceService = priceService;
            _contentLoaderWrapper = contentLoaderWrapper;
            _inventoryService = inventoryService;
        }

        public VsfSimpleProduct Map(VariationContent source)
        {
            var imageUrl = source.CommerceMediaCollection.FirstOrDefault()?.AssetLink.GetUrl();
            var thumbnail = UrlHelper.GetAsThumbnailUrl(imageUrl);
            var variantQuantity = GetTotalInventoryByEntry(source.Code);

            var productPrice = _priceService.GetDefaultPrice(source.ContentLink);

            var product = new VsfSimpleProduct
            {
                Id = source.ContentLink.ID,
                Name = source.DisplayName,
                UrlKey = source.RouteSegment,
                UrlPath = source.SeoUri,
                IsInStock = new VsfStock { IsInStock = true, Quantity = (int)variantQuantity },
                Sku = source.Code,
                TaxClassId = null,
                Image = imageUrl ?? "",
                Thumbnail = thumbnail,
                FinalPrice = productPrice,
                Price = productPrice,
                TypeId = "simple",
                SpecialPrice = null,
                NewsFromDate = null,
                NewsToDate = null,
                SpecialFromDate = null,
                SpecialToDate = null,
                CategoryIds = source.GetCategories().Select(x => x.ID.ToString()),
                Category = source.GetCategories().Select(x =>
                    new CategoryListItem { Id = x.ID, Name = _contentLoaderWrapper.Get<NodeContent>(x).DisplayName }),
                Status = 1,
                Visibility = 1, //hidden from vue store front list...
                Weight = 1,
                HasOptions = "0",
                RequiredOptions = "0",
                UpdatedAt = source.Changed,
                CreatedAt = source.Created
            };

            return product;
        }

        protected virtual decimal GetTotalInventoryByEntry(string code)
        {
            return _inventoryService.QueryByEntry(new[] { code }).Sum(x => x.PurchaseAvailableQuantity);
        }
    }
}