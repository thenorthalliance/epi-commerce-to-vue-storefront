using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DataMigration.Helpers;
using DataMigration.Input.Episerver.Attribute.Model;
using DataMigration.Input.Episerver.Common.Model;
using DataMigration.Input.Episerver.Common.Service;
using DataMigration.Input.Episerver.Product.Model;
using DataMigration.Output.ElasticSearch.Entity;
using EPiServer.Core;

namespace DataMigration.Input.Episerver.Attribute.Service
{
    public class AttributeService: ContentService
    {
        public override IEnumerable<CmsObjectBase> GetAll(ContentReference parentReference, CultureInfo cultureInfo)
        {
            var attributes = new List<EpiAttribute>();
            var productsService = ContentServiceFactory.Create(EntityType.Product);
            var products = (IEnumerable<EpiProduct>) productsService.GetAll(parentReference, cultureInfo);
            foreach (var product in products)
            {
                var variants = ContentHelper.GetVariants(product.ProductContent.ContentLink);
                
                foreach (var variant in variants)
                {
                    var variantProperties = ContentHelper.GetVariantVsfProperties(variant.ContentLink);
                    foreach (var variantProperty in variantProperties)
                    {
                        if (variantProperty.Value == null)
                        {
                            continue;
                        }
                        var attribute = attributes.FirstOrDefault(x => x.Id == variantProperty.PropertyDefinitionID);
                        if (attribute == null)
                        {
                            attributes.Add(new EpiAttribute
                            {
                                Name = variantProperty.Name,
                                Id = variantProperty.PropertyDefinitionID,
                                Values = new List<string>() { variantProperty.Value.ToString() }
                            });
                        }
                        else
                        {
                            if (!attribute.Values.Contains(variantProperty.Value))
                            {
                                attribute.Values.Add(variantProperty.Value.ToString());
                            }
                        }
                    }
                }
            }

            return attributes;
        }
    }
}
