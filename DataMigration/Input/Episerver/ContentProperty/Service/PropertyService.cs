using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DataMigration.Input.Episerver.Common.Helpers;
using DataMigration.Input.Episerver.Common.Model;
using DataMigration.Input.Episerver.Common.Service;
using DataMigration.Input.Episerver.ContentProperty.Model;
using DataMigration.Input.Episerver.Product.Model;
using DataMigration.Output.ElasticSearch.Entity;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;

namespace DataMigration.Input.Episerver.ContentProperty.Service
{
    public class PropertyService: IContentService
    {
        public IEnumerable<CmsObjectBase> GetAll(ContentReference parentReference, CultureInfo cultureInfo, int level = 2)
        {
            var properties = new List<EpiContentProperty>();
            var productsService = ContentServiceFactory.Create(EntityType.Product);
            var products = (IEnumerable<EpiProduct>) productsService.GetAll(parentReference, cultureInfo);
            foreach (var product in products)
            {
                var variants = product.ProductContent.GetVariants();
                foreach (var variant in variants)
                {
                    var variantProperties = ContentHelper.GetVariantVsfProperties(variant);
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
