using System.Linq;
using EPiServer.ServiceLocation;
using Mediachase.Commerce.InventoryService;

namespace DataMigration.Input.Episerver.Common.Service
{
    public class InventoryService
    {
        public static decimal GetTotalInventoryByEntry(string code)
        {
            var inventoryService = ServiceLocator.Current.GetInstance<IInventoryService>();
            return inventoryService.QueryByEntry(new[] { code }).Sum(x => x.PurchaseAvailableQuantity);
        }
    }
}
