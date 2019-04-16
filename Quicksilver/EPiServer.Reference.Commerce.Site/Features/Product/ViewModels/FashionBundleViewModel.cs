using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Reference.Commerce.Site.Features.Recommendations.ViewModels;
using System.Collections.Generic;
using EPiServer.Reference.Commerce.Shared.Models.Products;

namespace EPiServer.Reference.Commerce.Site.Features.Product.ViewModels
{
    public class FashionBundleViewModel : ProductViewModelBase
    {
        public FashionBundle Bundle { get; set; }
        public IEnumerable<EntryContentBase> Entries { get; set; }
    }
}