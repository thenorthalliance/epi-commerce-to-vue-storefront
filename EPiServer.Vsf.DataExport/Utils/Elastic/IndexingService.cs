using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer.Logging;
using EPiServer.Vsf.DataExport.Configuration;
using EPiServer.Vsf.DataExport.Model.Elastic;
using Nest;
using Attribute = EPiServer.Vsf.DataExport.Model.Elastic.Attribute;

namespace EPiServer.Vsf.DataExport.Utils.Elastic
{
    public class IndexingService
    {
        private readonly ILogger _logger = LogManager.GetLogger(typeof(IndexingService));

        private readonly VueStorefrontConfiguration _configuration;
        private readonly IElasticClient _elasticClient;
        private readonly ElasticIndexManager _indexManager;

        private readonly List<object> _itemsToIndex = new List<object>();
        private string _indexName;
        

        public IndexingService()
        {
            _configuration = VueStorefrontConfigurationManager.Configuration;
            _elasticClient = new ElasticClient(_configuration.GetElasticConnectionSettings());
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

        public bool CreateIndex()
        {
            _indexName = _indexManager.CreateIndex(x => x.Mappings(
                client => client
                    .Map<Product>(map => map.AutoMap())
                    .Map<Attribute>(map => map.AutoMap())
                    .Map<Category>(map => map.AutoMap())
            ));

            return _indexName != null;
        }

        public void AddForIndexing<T>(T items) where T : class
        {
            _itemsToIndex.Add(items);
        }

        public void AddForIndexing<T>(IEnumerable<T> items) where T : class
        {
            _itemsToIndex.AddRange(items);
        }

        public int IndexBatchIfReady()
        {
            if (_indexName == null)
            {
                throw new InvalidOperationException("Create index first.");
            }

            var count = 0;

            while (_itemsToIndex.Count >= _configuration.BulkIndexBatchSize)
            {
                count += _indexManager.BulkIndex(_itemsToIndex.Take(_configuration.BulkIndexBatchSize), _indexName);
                _itemsToIndex.RemoveRange(0, _configuration.BulkIndexBatchSize);
            }
            return count;
        }

        public int IndexBatchAll()
        {
            var count = IndexBatchIfReady();

            if (_itemsToIndex.Any())
            {
                count += _indexManager.BulkIndex(_itemsToIndex, _indexName);
                _itemsToIndex.Clear();
            }

            return count;
        }

        public bool ApplyChanges()
        {
            if (_indexName == null)
            {
                throw new InvalidOperationException("Create index first.");
            }
            if(_itemsToIndex.Any())
            {
                throw new InvalidOperationException("Index all items first.");
            }

            return _indexManager.SwitchAliasToIndex(_indexName);
        }
    }
}