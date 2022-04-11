using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoFixture;
using Data.Entities;
using EndToEnd.Utils;
using FluentAssertions;
using Newtonsoft.Json;
using RestSharp;
using Xunit;

namespace EndToEnd.EndPointTests;

public class CoinTests
{
    private readonly RestClient _client;
    private readonly Fixture _fixture;
    private const string Uri = "http://localHost:5555/coin";
    private readonly Coin _goldCoin = new (){ Name = "Gold", InGold = 1.0000M }; 
    private readonly Coin _silverCoin = new (){ Name = "Silver", InGold = 0.1000M }; 
    private readonly Coin _copperCoin = new (){ Name = "Copper", InGold = 0.0100M }; 
    private readonly Coin _platCoin = new (){ Name = "Plat", InGold = 10.0000M }; 
    
    public CoinTests()
    {
        _client = new RestClient();
        _fixture = new Fixture();
        Task.Run(Setup);
    }

    [Fact]
    public async Task ShouldCreateNewCoin()
    {
        var coin = _fixture.Create<Coin>();

        var request = new RestRequest(Uri)
            .AddJsonBody(coin);
        
        var response = await _client.ExecutePostAsync(request);
        response.IsSuccessful.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        Guid.TryParse(response.Content, out var x).Should().BeTrue();
    }

    [Fact]
    public async Task ShouldReturnCoinByName()
    {
        var request = new RestRequest($"{Uri}?name={_goldCoin.Name}");
        var response = await _client.GetAsync(request);
        var coin = JsonConvert.DeserializeObject<Coin>(response.Content);
        coin.Should().BeEquivalentTo(_goldCoin);
    }
    
    [Fact]
    public async Task ShouldReturnCoinById()
    {
        var request = new RestRequest($"{Uri}/{_silverCoin.Id}");
        var response = await _client.GetAsync(request);
        var coin = JsonConvert.DeserializeObject<Coin>(response.Content);
        coin.Should().BeEquivalentTo(_silverCoin);
    }

    //TODO refactor, as this might fail if insert test runs first.
    [Fact]
    public async Task ShouldReturnAll()
    {
        var request = new RestRequest(Uri);
        var response = await _client.GetAsync(request);
        var coins = JsonConvert.DeserializeObject<IEnumerable<Coin>>(response.Content);

        coins.Should().BeEquivalentTo(new List<Coin> { _goldCoin, _silverCoin, _copperCoin, _platCoin });
    }

    private async Task Setup()
    {
        var dataSeed = new DataSeed();
        await dataSeed.Reset<Coin>(Uri);
        
        _goldCoin.Id = Guid.Parse(await dataSeed.Insert(_goldCoin, Uri));
        _silverCoin.Id = Guid.Parse(await dataSeed.Insert(_silverCoin, Uri));
        _copperCoin.Id = Guid.Parse(await dataSeed.Insert(_copperCoin, Uri));
        _platCoin.Id = Guid.Parse(await dataSeed.Insert(_platCoin, Uri));
    }

    private void Delete(IEnumerable<Coin> coins)
    {
        foreach (var coin in coins)
        {
            var request = new RestRequest($"{Uri}?id={coin.Id}");
            _client.DeleteAsync(request);
        }
    }

    private void Reset()
    {
        var coins = GetAll();
        Delete(coins);
    }

    private IEnumerable<Coin> GetAll()
    {
        var request = new RestRequest(Uri);
        var response = _client.GetAsync(request).Result;
        var coins = JsonConvert.DeserializeObject<IEnumerable<Coin>>(response.Content);
        return coins;
    }
}