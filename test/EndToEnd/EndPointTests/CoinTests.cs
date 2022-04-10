using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using Data.Entities;
using FluentAssertions;
using Newtonsoft.Json;
using RestSharp;
using Xunit;

namespace EndToEnd.EndPointTests;

public class CoinTests
{
    private RestClient _client;
    private readonly Fixture _fixture;
    private const string Uri = "http://localHost:5555/coin";
    private readonly Coin _goldCoin = new Coin() { Name = "Gold", InGold = 1 }; 
    private readonly Coin _silverCoin = new Coin() { Name = "Silver", InGold = (decimal)0.1 }; 
    private readonly Coin _copperCoin = new Coin() { Name = "Copper", InGold = (decimal)0.01 }; 
    private readonly Coin _platCoin = new Coin() { Name = "Plat", InGold = 10 }; 
    
    public CoinTests()
    {
        _client = new RestClient();
        _fixture = new Fixture();
        
        _goldCoin.Id = _fixture.Create<Guid>();
        _silverCoin.Id = _fixture.Create<Guid>();
        _copperCoin.Id = _fixture.Create<Guid>();
        _platCoin.Id = _fixture.Create<Guid>();
        setup();
    }

    [Fact]
    public async Task ShouldCreateNewCoin()
    {
        var coin = _fixture.Create<Coin>();

        var request = new RestRequest(Uri)
            .AddJsonBody(coin);
        
        var response = await _client.ExecutePostAsync(request);
        response.IsSuccessful.Should().BeTrue();
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
        coin.Should().BeEquivalentTo(_silverCoin.Id);
    }

    [Fact]
    public async Task ShouldReturnAll()
    {
        var request = new RestRequest(Uri);
        var response = await _client.GetAsync(request);
        var coins = JsonConvert.DeserializeObject<IEnumerable<Coin>>(response.Content);

        coins.Should().Contain(new List<Coin> { _goldCoin, _silverCoin, _copperCoin, _platCoin });
    }

    private void setup()
    {
        foreach (var coin in new List<Coin> {_goldCoin, _silverCoin, _copperCoin, _platCoin })
        {
            var request = new RestRequest(Uri)
                .AddHeader("Content-Type", "Application/json")
                .AddJsonBody(coin);
        
            _client.ExecutePostAsync(request);
        }
    }
}