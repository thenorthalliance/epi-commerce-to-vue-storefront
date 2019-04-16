using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.PlugIn;
using EPiServer.Scheduler;
using EPiServer.ServiceLocation;
using EPiServer.Vsf.Core.Exporting;

namespace EPiServer.Reference.Commerce.VsfIntegration
{
    [ScheduledPlugIn(DisplayName = "Export to Vue Storefront")]
    public class VueStorefrontExportJob : ScheduledJobBase, IExtractedContentHandler
    {
        private readonly IContentExtractor _vsfExporter = ServiceLocator.Current.GetInstance<IContentExtractor>();
        private readonly IExtractedContentHandler _contentHandler = ServiceLocator.Current.GetInstance<IExtractedContentHandler>();

        private int _nodeExportCounter = 0;
        private int _productExportCounter = 0;
        
        public override string Execute()
        {
            _nodeExportCounter = 0;
            _productExportCounter = 0;

            _vsfExporter.Extract(this);
            return $"VSF export finished. {_nodeExportCounter} nodes and {_productExportCounter} product exported.";
        }

        public void OnBeginExtraction()
        {
            OnStatusChanged("Beginning VSF export.");
            _contentHandler.OnBeginExtraction();
        }

        public void OnNodeContent(NodeContent content, NodeContentBase parentNode)
        {
            _contentHandler.OnNodeContent(content, parentNode);
            _nodeExportCounter++;
            UpdateStatus();
        }

        public void OnProductContent(NodeContent parent, ProductContent product)
        {
            _contentHandler.OnProductContent(parent, product);
            _productExportCounter++;
            UpdateStatus();
        }

        public void OnFinishExtraction()
        {
            OnStatusChanged("Finishing VSF export.");
            _contentHandler.OnFinishExtraction();
        }

        public void UpdateStatus()
        {
            if ((_nodeExportCounter + _nodeExportCounter) % 100 == 0)
                OnStatusChanged($"{_nodeExportCounter} nodes and {_productExportCounter} product exported.");
        }
    }
}