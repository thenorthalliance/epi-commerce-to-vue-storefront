using System;
using System.Collections.Generic;
using System.Linq;
using DataMigration.Attributes;
using EPiServer;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Catalog.Linking;
using EPiServer.Core;
using EPiServer.ServiceLocation;

namespace DataMigration.Helpers
{
    public class ContentHelper
    {
        public static IEnumerable<ProductVariation> GetProductVariations(ContentReference referenceToProduct)
        {
            var relationRepository = ServiceLocator.Current.GetInstance<IRelationRepository>();
            var variations = relationRepository.GetChildren<ProductVariation>(referenceToProduct);
            return variations;
        }

        public static T GetContent<T>(ContentReference contentReference) where T : IContent
        {
            var contentLoader = ServiceLocator.Current.GetInstance<IContentLoader>();
            return contentLoader.Get<T>(contentReference);
        }

        public static IEnumerable<VariationContent> GetVariants(ContentReference productReference)
        {
            var contentLoader = ServiceLocator.Current.GetInstance<IContentLoader>();
            var variations = GetProductVariations(productReference);
            return variations.Select(x => contentLoader.Get<VariationContent>(x.Child));
        }

        public static IEnumerable<PropertyData> GetVariantVsfProperties(ContentReference variantReference)
        {
            var variant = GetContent<VariationContent>(variantReference);
            var propertiesNames = variant.GetType().GetProperties().Where(x => Attribute.IsDefined(x, typeof(VsfOptionAttribute))).Select(x => x.Name);
            return variant.Property.Where(x => propertiesNames.Contains(x.Name));
        }
    }
}
