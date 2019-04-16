using System.ComponentModel.DataAnnotations;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Catalog.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAnnotations;

namespace EPiServer.Reference.Commerce.Shared.Models.Products
{
    [CatalogContentType(
        GUID = "9568BD9F-FC9B-4004-A02C-5C0C90B68C43",
        DisplayName = "Fashion Package",
        MetaClassName = "FashionPackage",
        Description = "Displays a package, which is comparable to an individual SKU because Package item must be purchased as a whole.")]
    public class FashionPackage : PackageContent
    {
        [Searchable]
        [CultureSpecific]
        [Tokenize]
        [IncludeInDefaultSearch]
        [Display(Name = "Description", Order = 1)]
        public virtual XhtmlString Description { get; set; }
    }
}