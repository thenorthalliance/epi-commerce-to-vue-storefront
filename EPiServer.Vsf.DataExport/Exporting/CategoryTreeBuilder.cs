using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Vsf.DataExport.Model;

namespace EPiServer.Vsf.DataExport.Exporting
{
    public class CategoryTreeBuilder
    {
        private readonly EpiCategory _rootNode = new EpiCategory
        {
            Category = null,
            Level = 1,
            CategoryProductsCount = 0,
            SortOrder = 0,
            Children = new List<EpiCategory>()
        };

        private readonly Dictionary<Guid, EpiCategory> _categoryMap = new Dictionary<Guid, EpiCategory>();
        
        public void AddNode(NodeContent nodeContent, NodeContentBase parentNode)
        {
            if (NodeMapped(nodeContent))
                throw new Exception("NodeMapped(nodeContent)");

            var epiCategory = MkEpiCategory(nodeContent);

            if (MissingParrent(parentNode))
                InsertIntoRoot(epiCategory);
            else
                InsertIntoParent(epiCategory, parentNode);
            
            MapNode(epiCategory);
        }

        public void AddProductCount(NodeContent nodeContent)
        {
            if (!NodeMapped(nodeContent))
                throw new Exception("!NodeMapped(nodeContent)");

            _categoryMap[nodeContent.ContentGuid].CategoryProductsCount++;
        }

        public EpiCategory Build()
        {
            _categoryMap.Clear();
            return _rootNode;
        }

        private void InsertIntoRoot(EpiCategory node)
        {
            node.Level = 2;
            _rootNode.Children.Add(node);
        }
        
        private void InsertIntoParent(EpiCategory node, NodeContentBase parentNode)
        {
            var parentCategory = _categoryMap[parentNode.ContentGuid];
            var parentsChildred = parentCategory.Children;

            node.SortOrder = parentsChildred.Count;
            node.Level = parentCategory.Level + 1;

            parentsChildred.Add(node);
        }
        
        private void MapNode(EpiCategory node)
        {
            _categoryMap.Add(node.Category.ContentGuid, node);
        }

        private bool MissingParrent(NodeContentBase parentNode)
        {
            return !_categoryMap.ContainsKey(parentNode.ContentGuid);
        }

        private bool NodeMapped(NodeContent nodeContent)
        {
            return _categoryMap.ContainsKey(nodeContent.ContentGuid);
        }

        private EpiCategory MkEpiCategory(NodeContent node)
        {
            return new EpiCategory
            {
                Category = node,
                Level = 0,
                CategoryProductsCount = 0,
                SortOrder = _rootNode.Children.Count(),
                Children = new List<EpiCategory>()
            };
        }
    }
}