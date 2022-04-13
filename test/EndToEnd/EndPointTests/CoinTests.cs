using System;
using System.Collections.Generic;
using System.Linq;
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

public class CoinTests : IDisposable
{
    private readonly RestClient _client;
    private readonly Fixture _fixture;
    private readonly Uri _uri;
    private readonly ApiHelper _apiHelper;
    
    
    public CoinTests()
    {
        _uri = new Uri("http://localHost:5555/coin", UriKind.Absolute);
        _client = new RestClient();
        _fixture = new Fixture();
        _apiHelper = new ApiHelper(_uri);
    }

    [Fact]
    public async Task ShouldCreateNewCoinAndReturnGuid()
    {
        var coin = _fixture.Create<Coin>();

        var request = new RestRequest(_uri)
            .AddJsonBody(coin);
        
        var response = await _client.ExecutePostAsync(request);
        response.IsSuccessful.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        Guid.TryParse(response.Content, out var x).Should().BeTrue();
        await Delete(new List<Coin> {coin});
    }

    [Theory]
    [InlineData("", 1)]
    [InlineData("Gold", 0)]
    public async Task ShouldReturnBadWhenInvalidPassed(string name, decimal inGold)
    {
        var coin = new Coin {Name = name, InGold = inGold};

        var request = new RestRequest(_uri)
            .AddJsonBody(coin);
        
        var response = await _client.ExecutePostAsync(request);
        response.IsSuccessful.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ShouldReturnBadRequestWhenNameTaken()
    {
        var coin = _fixture.Create<Coin>();
        await _apiHelper.Insert(coin);
        
        var request = new RestRequest(_uri)
            .AddJsonBody(coin);
        
        var response = await _client.ExecutePostAsync(request);
        response.IsSuccessful.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        await Delete(new List<Coin> {coin});
    }

    // Get
    [Fact]
    public async Task ShouldReturnCoinByName()
    {
        var coins = _fixture.CreateMany<Coin>(3).ToList();
        await _apiHelper.Insert(coins);

        var expected = coins.First();
        
        var request = new RestRequest($"{_uri}?name={expected.Name}");
        var response = await _client.GetAsync(request);
        var actual = JsonConvert.DeserializeObject<Coin>(response.Content);
        actual.Should().BeEquivalentTo(expected);
        await Delete(coins);
    }

    [Fact]
    public async Task ShouldGetNotFoundById()
    {
        var request = new RestRequest($"{_uri}/{Guid.Empty}");
        var response = await _client.GetAsync(request);
        response.IsSuccessful.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task ShouldGetNotFoundByName()
    {
        var request = new RestRequest($"{_uri}?name=''");
        var response = await _client.GetAsync(request);
        response.IsSuccessful.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }



    [Fact]
    public async Task ShouldReturnCoinById()
    {
        var coins = _fixture.CreateMany<Coin>(3).ToList();
        await _apiHelper.Insert(coins);

        var expected = coins.First();
        
        var request = new RestRequest($"{_uri}/{expected.Id}");
        var response = await _client.GetAsync(request);
        var actual = JsonConvert.DeserializeObject<Coin>(response.Content);
        actual.Should().BeEquivalentTo(expected);
        await Delete(coins);
    }

    [Fact]
    public async Task ShouldReturnAll()
    {
        var coins = _fixture.CreateMany<Coin>(10).ToList();
        await _apiHelper.Insert(coins);

        var request = new RestRequest(_uri);
        var response = await _client.GetAsync(request);
        var actual = JsonConvert.DeserializeObject<IEnumerable<Coin>>(response.Content);

        coins.Select(expectation => actual.Should().ContainEquivalentOf(expectation));
        
        await Delete(coins);
    }

    private async Task Delete(IEnumerable<Coin> coins)
    {
        await _apiHelper.Delete(coins.Select(x => x.Id));
    }
    
    public void Dispose()
    {
        Task.Run(() => _apiHelper.Reset<Coin>());
        _client.Dispose();
    }
}