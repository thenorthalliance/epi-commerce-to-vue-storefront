using System;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Reference.Commerce.Shared.Models.Products;
using EPiServer.Vsf.DataExport.Mapping;

namespace EPiServer.Reference.Commerce.VsfIntegration.Mapping
{
    public class QuicksilverNodeMapper : CategoryBaseMapper
    {
        protected override string GetDescription(NodeContent nodeContent)
        {
            if (nodeContent.GetOriginalType() == typeof(FashionNode))
            {
                var category = (FashionNode)nodeContent;
                return category.Description?.ToString();
            }

            throw new ArgumentException("Source not supported");
        }
    }
}
