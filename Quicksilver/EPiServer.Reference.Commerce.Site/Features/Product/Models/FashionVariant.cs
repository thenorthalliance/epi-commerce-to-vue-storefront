using System.ComponentModel.DataAnnotations;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Catalog.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.Vsf.DataExport.Attributes;

namespace EPiServer.Reference.Commerce.Site.Features.Product.Models
{
    [CatalogContentType(
        GUID = "c1bc1c4e-4a46-4a03-8e90-71bec3576aff",
        MetaClassName = "FashionVariant",
        DisplayName = "Fashion Variant",
        Description = "Display fashion variant")]
    public class FashionVariant : VariationContent
    {
        [VsfOption]
        [Searchable]
        [Tokenize]
        [IncludeInDefaultSearch]
        [BackingType(typeof(PropertyString))]
        [Display(Name = "Size", Order = 2)]
        public virtual string Size { get; set; }

        [VsfOption]
        [Searchable]
        [CultureSpecific]
        [Tokenize]
        [IncludeInDefaultSearch]
        [BackingType(typeof(PropertyString))]
        [Display(Name = "Color", Order = 1)]
        public virtual string Color { get; set; }
    }
}