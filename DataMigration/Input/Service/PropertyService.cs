using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DataMigration.Input.Model;
using DataMigration.Utils.Epi;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;

namespace DataMigration.Input.Service
{
    public class PropertyService : IContentService<EpiContentProperty>
    {
        private readonly ContentService _contentService;
        private readonly IContentService<EpiProduct> _productService;

        public PropertyService(ContentService contentService, IContentService<EpiProduct> productService)
        {
            _contentService = contentService;
            _productService = productService;
        }


        public IEnumerable<EpiContentProperty> GetAll(ContentReference parentReference, CultureInfo cultureInfo, int level = 2)
        {
            var properties = new List<EpiContentProperty>();
            var products =_productService.GetAll(parentReference, cultureInfo);
            foreach (var product in products)
            {
                var variants = product.ProductContent.GetVariants();
                foreach (var variant in variants)
                {
                    var variantProperties = _contentService.GetVariantVsfProperties(variant);
                    foreach (var variantProperty in variantProperties)
                    {
                        if (variantProperty.Value == null)
                        {
                            continue;
                        }
                        var existingProperty = properties.FirstOrDefault(x => x.Id == variantProperty.PropertyDefinitionID);
                        if (existingProperty == null)
                        {
                            properties.Add(new EpiContentProperty
                            {
                                Name = variantProperty.Name,
                                Id = variantProperty.PropertyDefinitionID,
                                Values = new List<string>() { variantProperty.Value.ToString() }
                            });
                        }
                        else
                        {
                            if (!existingProperty.Values.Contains(variantProperty.Value))
                            {
                                existingProperty.Values.Add(variantProperty.Value.ToString());
                            }
                        }
                    }
                }
            }

            return properties;
        }
    }
}
