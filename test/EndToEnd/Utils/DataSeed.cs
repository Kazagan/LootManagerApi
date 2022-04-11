using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Entities;
using Newtonsoft.Json;
using RestSharp;

namespace EndToEnd.Utils;

public class DataSeed
{
    private readonly RestClient _client;
    public DataSeed()
    {
        _client = new RestClient();
    }

    public async Task<string> Insert<TEntity>(TEntity entity, string uri) where TEntity : Entity
    {
        var request = new RestRequest(uri)
            .AddJsonBody(entity);

        var response = await _client.PostAsync(request);
        return response.Content!;
    }

    public async Task Reset<TEntity>(string uri) where TEntity : Entity
    {
        var ids = await GetAll<TEntity>(uri);
        await Delete(ids, uri);
    }
    
    private async Task Delete(IEnumerable<Guid> ids, string uri)
    {
        foreach (var id in ids)
        {
            var request = new RestRequest($"{uri}?id={id}");
            await _client.DeleteAsync(request);
        }
    }

    private async Task<IEnumerable<Guid>> GetAll<TEntity>(string uri) where TEntity : Entity
    {
        var request = new RestRequest(uri);
        var response = await _client.GetAsync(request);
        var entities = JsonConvert.DeserializeObject<IEnumerable<TEntity>>(response.Content);
        return entities.Select(x => x.Id);
    }
}