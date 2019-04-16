using System.Linq;
using EPiServer.Core;
using Mediachase.Commerce;
using Mediachase.Commerce.Pricing;

namespace EPiServer.Vsf.DataExport.Utils.Epi
{
    public class VsfPriceService : IVsfPriceService
    {
        private readonly ICurrentMarket _currentMarket;
        private readonly IPriceDetailService _priceDetailService;

        public VsfPriceService(ICurrentMarket currentMarket, IPriceDetailService priceDetailService)
        {
            _currentMarket = currentMarket;
            _priceDetailService = priceDetailService;
        }

        public decimal GetDefaultPrice(ContentReference reference)
        {
            var currentMarket = _currentMarket.GetCurrentMarket();
            var priceDetailValues = _priceDetailService.List(reference);
         
            var price = priceDetailValues.FirstOrDefault(d => d.MarketId == currentMarket.MarketId);
            return price?.UnitPrice.Amount ?? 0.0m;
        }
    }
}
