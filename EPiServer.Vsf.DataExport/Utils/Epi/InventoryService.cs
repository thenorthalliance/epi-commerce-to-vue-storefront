using System.Linq;
using Mediachase.Commerce.InventoryService;

namespace EPiServer.Vsf.DataExport.Utils.Epi
{
    public class InventoryService
    {
        private readonly IInventoryService _inventoryService;

        public InventoryService(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        public decimal GetTotalInventoryByEntry(string code)
        {
            return _inventoryService.QueryByEntry(new[] { code }).Sum(x => x.PurchaseAvailableQuantity);
        }
    }
}
