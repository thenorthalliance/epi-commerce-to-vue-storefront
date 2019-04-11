using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Vsf.Core.ApiBridge.Adapter;
using EPiServer.Vsf.Core.ApiBridge.Model.Stock;
using Mediachase.Commerce.Catalog;
using Mediachase.Commerce.InventoryService;

namespace EPiServer.Reference.Commerce.VsfIntegration
{
    public class QuickSilverStockAdapter : IStockAdapter
    {
        private readonly IInventoryService _inventoryService;
        private readonly IContentLoader _contentLoader;
        private readonly ReferenceConverter _referenceConverter;

        public QuickSilverStockAdapter(IInventoryService inventoryService, IContentLoader contentLoader, ReferenceConverter referenceConverter)
        {
            _inventoryService = inventoryService;
            _contentLoader = contentLoader;
            _referenceConverter = referenceConverter;
        }

        [AllowAnonymous]
        public async Task<StockCheck> Check(string code)
        {
            var variationLinkt = _referenceConverter.GetContentLink(code);
            var variation = _contentLoader.Get<VariationContent>(variationLinkt);
            var productLink = variation.GetParentProducts().FirstOrDefault();
//            var product = _contentLoader.Get<ProductContent>(productLink);
            
            var qauantity = _inventoryService.QueryByEntry(new[] {code}).Sum(x => x.PurchaseAvailableQuantity);
            return new StockCheck
            {
                ItemId = variationLinkt.ID,
                ProductId = productLink.ID,
                StockId = 1, //todo
                Qty = qauantity,
                IsInStock = qauantity > 0,
                IsQtyDecimal = false,
                ShowDefaultNotificationMessage = false,
                UseConfigMinQty = true,
                MinQty = variation.MinQuantity ?? 0,
                UseConfigMinSaleQty = true,
                MinSaleQty = 1,
                UseConfigMaxSaleQty = true,
                MaxSaleQty = variation.MaxQuantity ?? 10000,
                UseConfigBackorders = true,
                Backorders = 0,
                UseConfigNotifyStockQty = true,
                NotifyStockQty = 1,
                UseConfigQtyIncrements = true,
                QtyIncrements = 0,
                UseConfigEnableQtyInc = true,
                EnableQtyIncrements = false,
                UseConfigManageStock = true,
                ManageStock = true,
                LowStockDate = null,
                IsDecimalDivided = false,
                StockStatusChangedAuto = 0
            };
        }
    }
}