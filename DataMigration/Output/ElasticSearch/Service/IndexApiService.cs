using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DataMigration.Output.ElasticSearch.Entity.Category.Model;
using DataMigration.Output.ElasticSearch.Entity.Product.Model;
using Nest;

namespace DataMigration.Output.ElasticSearch.Service
{
    public class IndexApiService
    {
        private readonly string _indexName;
        private readonly IElasticClient _client;

        public IndexApiService(string indexName, string serverUri)
        {
            _indexName = indexName;
            var settings = new ConnectionSettings(new Uri(serverUri)).DefaultIndex(indexName);
            _client = new ElasticClient(settings);
            
        }

        public async Task CreateIndex()
        {
            _client.CreateIndex(_indexName);
            _client.Map<Product>(c => c.AutoMap());
            _client.Map<Attribute>(c => c.AutoMap());
            _client.Map<Category>(c => c.AutoMap());
        }

        public async Task<dynamic> IndexSingel<T>(T doc) where T: class
        {
            var response = await _client.IndexDocumentAsync(doc);
            return new {Status = response.Result.ToString("g"), Error = response.OriginalException?.ToString()};
        }

        public async Task<dynamic[]> IndexMany<T>(IEnumerable<T> items) where T:class
        {
            var entities = items.Select(item => IndexSingel(item)).ToList();
            return await Task.WhenAll(entities);
        }

        public Stream Serialize<T>(T document) where T: class
        {
            var memoryStream = new MemoryStream();
            _client.SourceSerializer.Serialize(document, memoryStream);
            memoryStream.Position = 0;
            return memoryStream;
        }
    }
}