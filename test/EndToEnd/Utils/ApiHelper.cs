using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Entities;
using Newtonsoft.Json;
using RestSharp;

namespace EndToEnd.Utils;

public class ApiHelper
{
    private readonly RestClient _client;

    public ApiHelper()
    {
        _client = new RestClient();
    }

    // TODO figure how to use this to lessen code duplication without expected bad requests throwing errors.
    public async Task<RestResponse> Insert<TEntity>(Uri uri, TEntity entity) where TEntity : Entity
    {
        var request = new RestRequest(uri)
            .AddJsonBody(entity);

        return await _client.PostAsync(request);
    }

    public async Task<RestResponse> Update<TEntity>(Uri uri, TEntity entity) where TEntity : Entity
    {
        var request = new RestRequest(uri)
            .AddJsonBody(entity);

        var response = await _client.PutAsync(request);
        return response;
    }

    public async Task<TEntity> GetById<TEntity>(Uri uri, Guid guid) where TEntity : Entity
    {
        var request = new RestRequest($"{uri}/{guid}");
        var response = await _client.GetAsync(request);
        var result = JsonConvert.DeserializeObject<TEntity>(response.Content);
        return result;
    }

    public async Task Insert<TEntity>(Uri uri, IEnumerable<TEntity> entities) where TEntity : Entity
    {
        foreach (var entity in entities)
        {
            await Insert(uri, entity);
        }
    }

    public async Task Reset<TEntity>(Uri uri) where TEntity : Entity
    {
        var ids = await GetAll<TEntity>(uri);
        foreach (var id in ids)
        {
            await Delete(uri, id);
        }
    }

    public async Task Delete(Uri uri, Guid id)
    {
        var request = new RestRequest($"{uri}?id={id}");
        await _client.DeleteAsync(request);
    }

    private async Task<IEnumerable<Guid>> GetAll<TEntity>(Uri uri) where TEntity : Entity
    {
        var request = new RestRequest(uri);
        var response = await _client.GetAsync(request);
        var entities = JsonConvert.DeserializeObject<IEnumerable<TEntity>>(response.Content);
        return entities.Select(x => x.Id);
    }
}