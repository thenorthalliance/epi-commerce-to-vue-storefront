using System.Linq;
using EPiServer.ServiceLocation;
using Mediachase.Commerce.InventoryService;

namespace DataMigration.Helpers
{
    public class InventoryHelper
    {
        public static decimal GetTotalInventoryByEntry(string code)
        {
            var inventoryService = ServiceLocator.Current.GetInstance<IInventoryService>();
            return inventoryService.QueryByEntry(new[] { code }).Sum(x => x.PurchaseAvailableQuantity);
        }
    }
}
