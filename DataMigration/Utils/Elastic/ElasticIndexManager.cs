using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer.Logging;
using Nest;

namespace DataMigration.Utils.Elastic
{
    public class ElasticIndexManager
    {
        private readonly ILogger _logger = LogManager.GetLogger(typeof(ElasticIndexManager));

        public readonly IElasticClient _client;
        private readonly string _aliasName;

        public ElasticIndexManager(IElasticClient client, string aliasName)
        {
            _client = client;
            _aliasName = aliasName;
        }

        public string CreateIndex(Func<CreateIndexDescriptor, ICreateIndexRequest> selector = null)
        {
            var newIndexName = $"{_aliasName}-{DateTime.Now:yyyyMMddHHmm}";
            var response = _client.CreateIndex(newIndexName, selector);
            return response.IsValid ? newIndexName : null;
        }

        public int BulkIndex<T>(IEnumerable<T> data, string indexName) where T : class
        {
            var descriptor = new BulkDescriptor();
            descriptor.IndexMany(data, (desc, content) => desc.Type(content.GetType().Name.ToLower()).Index(indexName));
            var response = _client.Bulk(descriptor);
            return response.Items.Count;
        }

        public bool SwitchAliasToIndex(string indexName)
        {
            var catAliases = _client.CatAliases(new CatAliasesRequest(_aliasName));
            var oldIndexes = catAliases.Records.Select(x => x.Index);

            var response = _client.Alias(a => RemoveIndexesFromAlias(a, oldIndexes).Add(i => i.Index(indexName).Alias(_aliasName)));
            if (!response.IsValid)
            {
                return false;
            }

            foreach (var index in oldIndexes)
            {
                try
                {
                    _client.DeleteIndexAsync(new DeleteIndexRequest(index));
                }
                catch (Exception ex)
                {
                    _logger.Error($"Unable to delete index: {index}", ex);
                }
            }

            return true;
        }

        private BulkAliasDescriptor RemoveIndexesFromAlias(BulkAliasDescriptor descriptor, IEnumerable<string> indexes)
        {
            foreach (var index in indexes)
            {
                descriptor = descriptor.Remove(i => i.Index(index).Alias(_aliasName));
            }
            return descriptor;
        }
    }
}
