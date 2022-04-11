using System;
using System.Collections.Generic;
using System.Net;
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
    private readonly RestClient _client;
    private readonly Fixture _fixture;
    private const string Uri = "http://localHost:5555/coin";
    private readonly Coin _goldCoin = new Coin() { Name = "Gold", InGold = 1.0000M }; 
    private readonly Coin _silverCoin = new Coin() { Name = "Silver", InGold = 0.1000M }; 
    private readonly Coin _copperCoin = new Coin() { Name = "Copper", InGold = 0.0100M }; 
    private readonly Coin _platCoin = new Coin() { Name = "Plat", InGold = 10.0000M }; 
    
    public CoinTests()
    {
        _client = new RestClient();
        _fixture = new Fixture();
        Reset();
        
        _goldCoin.Id = Guid.Parse(Insert(_goldCoin));
        _silverCoin.Id = Guid.Parse(Insert(_silverCoin));
        _copperCoin.Id = Guid.Parse(Insert(_copperCoin));
        _platCoin.Id = Guid.Parse(Insert(_platCoin));
        Console.WriteLine();
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

    private string Insert(Coin coin)
    {
        var request = new RestRequest(Uri)
            .AddJsonBody(coin);
        var response = _client.PostAsync(request).Result;
        return response.Content!;
    }
}