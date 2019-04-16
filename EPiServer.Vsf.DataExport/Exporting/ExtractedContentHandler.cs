using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Vsf.Core.Exporting;
using EPiServer.Vsf.Core.Mapping;
using EPiServer.Vsf.DataExport.Model;
using EPiServer.Vsf.DataExport.Utils.Epi;

namespace EPiServer.Vsf.DataExport.Exporting
{   
    public class ExtractedContentHandler<TProduct> : IExtractedContentHandler where TProduct: VsfBaseProduct
    {
        private readonly IIndexingService _indexingService;
        private readonly ContentPropertyReader _contentPropertyLoader;
        private readonly IMapper<ProductContent, TProduct> _productMapper;
        private readonly IMapper<EpiCategory, VsfCategory> _categoryMapper;
        private readonly IMapper<EpiContentProperty, VsfAttribute> _attributeMapper;
        private readonly CategoryTreeBuilder _categoryTreeBuilder = new CategoryTreeBuilder();

        public ExtractedContentHandler(
            IIndexingService indexingService,
            IContentLoaderWrapper contentLoaderWrapper,
            IMapper<ProductContent, TProduct> productMapper,
            IMapper<EpiCategory, VsfCategory> categoryMapper,
            IMapper<EpiContentProperty, VsfAttribute> attributeMapper)
        {
            _indexingService = indexingService;
            _productMapper = productMapper;
            _categoryMapper = categoryMapper;
            _attributeMapper = attributeMapper;

            _contentPropertyLoader = new ContentPropertyReader(contentLoaderWrapper);
        }

        public void OnNodeContent(NodeContent content, NodeContentBase parentNode)
        {
            _categoryTreeBuilder.AddNode(content, parentNode);
        }

        public void OnProductContent(NodeContent parent, ProductContent productContent)
        {
            var vsfProduct = _productMapper.Map(productContent);

            _categoryTreeBuilder.AddProductCount(parent);
            _indexingService.AddForIndexing(vsfProduct);
            _contentPropertyLoader.ReadProperties(productContent);
        }

        public void OnBeginExtraction()
        {
            _indexingService.CreateIndex();
        }

        public void OnFinishExtraction()
        {
            var rootNode = _categoryTreeBuilder.Build();

            IndexCategoryTree(rootNode);
            IndexAttributes();

            _indexingService.ApplyChanges();
        }
        

        private void IndexCategoryTree(EpiCategory node)
        {
            foreach (var child in node.Children)
            {
                var vsfCategory = _categoryMapper.Map(child);
                _indexingService.AddForIndexing(vsfCategory);
                IndexCategoryTree(child);
            }
        }

        private void IndexAttributes()
        {
            foreach (var property in _contentPropertyLoader.GetProperties())
            {
                var vsfAttribute = _attributeMapper.Map(property);
                _indexingService.AddForIndexing(vsfAttribute);
            }
        }
    }
}