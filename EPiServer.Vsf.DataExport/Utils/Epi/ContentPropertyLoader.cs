using System.Collections.Generic;
using System.Linq;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Vsf.DataExport.Model;

namespace EPiServer.Vsf.DataExport.Utils.Epi
{
    public class ContentPropertyLoader
    {
        private readonly IContentLoader _contentLoader;
        private readonly List<EpiContentProperty> _epiContentProperties = new List<EpiContentProperty>();

        
        public ContentPropertyLoader(IContentLoader contentLoader)
        {
            _contentLoader = contentLoader;
        }

        public IEnumerable<EpiContentProperty> GetProperties() => _epiContentProperties;

        public void Clear() => _epiContentProperties.Clear();


        public void LoadProperties(IEnumerable<ProductContent> products)
        {
            foreach (var product in products)
            {
                LoadPropertry(product);
            }
        }

        public void LoadPropertry(ProductContent product)
        {
            var variants = product.GetVariants();
            foreach (var variant in variants)
            {
                var variantProperties = _contentLoader.GetVariantVsfProperties(variant);
                foreach (var variantProperty in variantProperties)
                {
                    if (variantProperty.Value == null)
                    {
                        continue;
                    }

                    var existingProperty =
                        _epiContentProperties.FirstOrDefault(x => x.Id == variantProperty.PropertyDefinitionID);
                    if (existingProperty == null)
                    {
                        _epiContentProperties.Add(new EpiContentProperty
                        {
                            Name = variantProperty.Name,
                            Id = variantProperty.PropertyDefinitionID,
                            Values = new List<string>() {variantProperty.Value.ToString()}
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
    }
}