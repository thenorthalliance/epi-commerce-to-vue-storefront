using System;
using System.Linq;
using EPiServer.Core;
using Mediachase.Commerce;
using Mediachase.Commerce.Catalog;
using Mediachase.Commerce.Pricing;

namespace EPiServer.Vsf.DataExport.Utils.Epi
{
    public class PriceService
    {
        private readonly IPriceService _priceService;
        private readonly ICurrentMarket _currentMarket;
        private readonly IPriceDetailService _priceDetailService;

        public PriceService(
            IPriceService priceService,
            ICurrentMarket currentMarket,
            IPriceDetailService priceDetailService)
        {
            _priceService = priceService;
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

        public decimal GetDefaultPrice(string code)
        {
            var currentMarket = _currentMarket.GetCurrentMarket();

            var price = _priceService.GetDefaultPrice(currentMarket.MarketId, DateTime.Now, new CatalogKey(code), currentMarket.DefaultCurrency);
            return price?.UnitPrice.Amount ?? 0.0m;
        }
    }
}
