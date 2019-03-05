using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Newtonsoft.Json;

namespace DataMigration.Output.ElasticSearch.Service
{
    public class IndexApiService
    {
        private readonly string _indexName;
        private readonly IElasticLowLevelClient _client;

        public IndexApiService(string indexName, string serverUrl = null)
        {
            _indexName = indexName;

            if (!string.IsNullOrWhiteSpace(serverUrl))
            {
                var settings = new ConnectionConfiguration(new Uri(serverUrl));
                _client = new ElasticLowLevelClient(settings);
            }
            else
            {
                _client = new ElasticLowLevelClient();
            }
        }

        public async Task<dynamic> SendAsync(Entity.Entity entity, string type)
        {
            var clientResponse = await _client.IndexAsync<StringResponse>(_indexName, type, entity.Id.ToString(), PostData.String(JsonConvert.SerializeObject(entity)));
            return clientResponse.Success ? new { EntityId = entity.Id, Status = "Success", Exception = string.Empty } : new { EntityId = entity.Id, Status = "Failed", Exception = clientResponse.OriginalException.Message };
        }

        public async Task<dynamic[]> SendAsync(IEnumerable<Entity.Entity> items, string type)
        {
            var entities = items.Select(item => SendAsync(item, type)).ToList();

            return await Task.WhenAll(entities);
        }
    }
}