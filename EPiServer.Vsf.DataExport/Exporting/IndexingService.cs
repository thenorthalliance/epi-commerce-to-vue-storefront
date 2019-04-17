using System;
using System.Collections.Generic;
using System.Linq;
using Elasticsearch.Net;
using EPiServer.Logging;
using EPiServer.Vsf.Core.Exporting;
using EPiServer.Vsf.DataExport.Configuration;
using EPiServer.Vsf.DataExport.Model;
using Nest;

namespace EPiServer.Vsf.DataExport.Exporting
{
    public class IndexingService<TProduct> : IIndexingService where TProduct: VsfBaseProduct
    {
        private readonly ILogger _logger = LogManager.GetLogger(typeof(IndexingService<TProduct>));

        private readonly VsfExporterConfiguration _configuration;
        private readonly IElasticClient _elasticClient;
        private readonly ElasticIndexManager _indexManager;

        private readonly List<object> _itemsToIndex = new List<object>();
        private string _indexName;


        public IndexingService(VsfExporterConfiguration configuration)
        {
            _configuration = configuration;
            _elasticClient = new ElasticClient(GetElasticConnectionSettings(_configuration.ElasticServerUrls, _configuration.ElasticUserName, _configuration.ElasticPassword));
            _indexManager = new ElasticIndexManager(_elasticClient, _configuration.IndexAliasName);
        }

        public bool IsServiceAvailable()
        {
            try
            {
                var root = _elasticClient.RootNodeInfo();
                return root.IsValid;
            }
            catch (Exception ex)
            {
                _logger.Error("Elasticsearch not available.", ex);
                return false;
            }
        }

        public void CreateIndex()
        {
            _indexName = _indexManager.CreateIndex(x => x.Mappings(
                client => client
                    .Map<TProduct>(map => map.AutoMap()
                        .Properties(MapConfigurableChildrenSkuProperty))
                    .Map<VsfAttribute>(map => map.AutoMap())
                    .Map<VsfCategory>(map => map.AutoMap())
            ));
        }

        public void AddForIndexing<T>(T items) where T : class
        {
            _itemsToIndex.Add(items);
            IndexBatchIfReady();
        }

        public void AddForIndexing<T>(IEnumerable<T> items) where T : class
        {
            _itemsToIndex.AddRange(items);
            IndexBatchIfReady();
        }
        
        public void ApplyChanges()
        {
            if (_indexName == null)
                throw new InvalidOperationException("Create index first.");

            IndexBatchAll();
            _indexManager.SwitchAliasToIndex(_indexName);
        }

        private PropertiesDescriptor<TProduct> MapConfigurableChildrenSkuProperty(PropertiesDescriptor<TProduct> propertiesDescriptor)
        {
            return propertiesDescriptor.Object<object>(s =>
            {
                return s.Name("configurable_children").Properties(ps2 =>
                {
                    return ps2.Keyword(kw => kw.Name("sku"));
                });
            });
        }

        private void IndexBatchIfReady()
        {
            if (_indexName == null)
            {
                throw new InvalidOperationException("Create index first.");
            }
            
            while (_itemsToIndex.Count >= _configuration.BulkIndexBatchSize)
            {
                _indexManager.BulkIndex(_itemsToIndex.Take(_configuration.BulkIndexBatchSize), _indexName);
                _itemsToIndex.RemoveRange(0, _configuration.BulkIndexBatchSize);
            }
        }

        private void IndexBatchAll()
        {
            IndexBatchIfReady();

            if (_itemsToIndex.Any())
            {
                _indexManager.BulkIndex(_itemsToIndex, _indexName);
                _itemsToIndex.Clear();
            }
        }
        
        public static IConnectionSettingsValues GetElasticConnectionSettings(string elasticServerUrls, string elasticUserName, string elasticPassword)
        {
            var nodes = elasticServerUrls.Split(';').Select(x => new Uri(x));
            var connectionPool = new StaticConnectionPool(nodes);
            var connectionSettings = new ConnectionSettings(connectionPool)
                .DefaultMappingFor<TProduct>(m => m.TypeName("product"))
                .DefaultMappingFor<VsfAttribute>(m => m.TypeName("attribute"))
                .DefaultMappingFor<VsfCategory>(m => m.TypeName("category"));
            
            if (!string.IsNullOrEmpty(elasticUserName) || !string.IsNullOrEmpty(elasticPassword))
            {
                connectionSettings.BasicAuthentication(elasticUserName, elasticPassword);
            }

            return connectionSettings;
        }
    }
} 