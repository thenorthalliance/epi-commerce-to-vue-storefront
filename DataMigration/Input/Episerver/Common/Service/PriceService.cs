using System.Linq;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using Mediachase.Commerce.Pricing;

namespace DataMigration.Input.Episerver.Common.Service
{
    public class PriceService
    {
        public static int GetPrice(ContentReference priceReference)
        {
            var priceService = ServiceLocator.Current.GetInstance<IPriceDetailService>();
            var price = priceService.List(priceReference).FirstOrDefault();
            if (price != null) return (int)price.UnitPrice.Amount;
            return 0;
        }
    }
}
