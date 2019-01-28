using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataMigration.Input.Episerver.Common.Model;
using DataMigration.Mapper;
using DataMigration.Output.ElasticSearch.Entity;
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

        public async Task<dynamic> SendAsync(CmsObjectBase cmsObject)
        {
            var mapper = MapperFactory.Create(cmsObject.EntityType);

            var elasticSearchEntity = mapper.Map(cmsObject);
            
            var clientResponse = await _client.IndexAsync<StringResponse>(_indexName, cmsObject.EntityType.DisplayName(), elasticSearchEntity.Id.ToString(), PostData.String(JsonConvert.SerializeObject(elasticSearchEntity)));

            return clientResponse.Success ? new { EntityId = elasticSearchEntity.Id, Status = "Success", Exception = string.Empty } : new { EntityId = elasticSearchEntity.Id, Status = "Failed", Exception = clientResponse.OriginalException.Message };
        }

        public async Task<dynamic[]> SendAsync(IEnumerable<CmsObjectBase> items)
        {
            var entities = new List<Task<dynamic>>();

            foreach (var item in items)
            {
                entities.Add(SendAsync(item));
            }

            return await Task.WhenAll(entities);
        }
    }
}