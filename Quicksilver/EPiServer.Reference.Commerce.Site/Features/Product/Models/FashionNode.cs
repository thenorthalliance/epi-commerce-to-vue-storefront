using System.ComponentModel.DataAnnotations;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Catalog.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.Reference.Commerce.Site.Features.Product.Models;

namespace EPiServer.Reference.Commerce.Site.Features.Product.Models
{
    [CatalogContentType(
        GUID = "a23da2a1-7843-4828-9322-c63e28059f6a",
        MetaClassName = "FashionNode",
        DisplayName = "Fashion Node",
        Description = "Display fashion products.")]
    [AvailableContentTypes(Include = new[] 
    {
        typeof(FashionProduct),
        typeof(FashionPackage),
        typeof(FashionBundle),
        typeof(FashionVariant),
        typeof(NodeContent)
    })]
    public class FashionNode : NodeContent
    {
        [Searchable]
        [CultureSpecific]
        [Tokenize]
        [IncludeInDefaultSearch]
        [Display(Name = "Description", Order = 2)]
        public virtual XhtmlString Description { get; set; }
    }
}