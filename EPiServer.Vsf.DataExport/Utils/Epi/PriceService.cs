using System;
using Mediachase.Commerce;
using Mediachase.Commerce.Catalog;
using Mediachase.Commerce.Pricing;

namespace EPiServer.Vsf.DataExport.Utils.Epi
{
    public class PriceService
    {
        private readonly IPriceService _priceService;
        private readonly ICurrentMarket _currentMarket;

        public PriceService(
            IPriceService priceService,
            ICurrentMarket currentMarket)
        {
            _priceService = priceService;
            _currentMarket = currentMarket;
        }

        public decimal GetDefaultPrice(string code)
        {
            var currentMarket = _currentMarket.GetCurrentMarket();
            var price = _priceService.GetDefaultPrice(currentMarket.MarketId, DateTime.Now, new CatalogKey(code), currentMarket.DefaultCurrency);
            return price?.UnitPrice.Amount ?? 0.0m;
        }
    }
}
