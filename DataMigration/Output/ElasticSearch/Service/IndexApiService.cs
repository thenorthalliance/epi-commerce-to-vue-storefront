﻿using System;
using System.Collections.Generic;
using DataMigration.Configuration;
using DataMigration.Output.ElasticSearch.Entity.Category.Model;
using DataMigration.Output.ElasticSearch.Entity.Product.Model;
using DataMigration.Utils;
using EPiServer.Logging;
using Nest;

namespace DataMigration.Output.ElasticSearch.Service
{
    public class IndexApiService
    {
        private readonly ILogger _logger = LogManager.GetLogger(typeof(IndexApiService));

        private readonly VueStorefrontConfiguration _configuration;
        private readonly IElasticClient _elasticClient;
        private readonly ElasticIndexManager _indexManager;

        private string _indexName;

        public IndexApiService()
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
                    .Map<Entity.Attribute.Model.Attribute>(map => map.AutoMap())
                    .Map<Category>(map => map.AutoMap())
            ));

            return _indexName != null;
        }

        public int IndexMany<T>(IEnumerable<T> items) where T : class
        {
            if (_indexName == null)
            {
                throw new InvalidOperationException("Create index first.");
            }

            var count = 0;
            foreach (var batch in items.Chunk(_configuration.BulkIndexBatchSize))
            {
                count += _indexManager.BulkIndex(batch, _indexName);
            }
            return count;

        }

        public bool ApplyChanges()
        {
            if (_indexName == null)
            {
                throw new InvalidOperationException("Create index first.");
            }

            return _indexManager.SwitchAliasToIndex(_indexName);
        }
    }
}