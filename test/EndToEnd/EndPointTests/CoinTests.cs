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

public class CoinTests
{
    private readonly RestClient _client;
    private readonly Fixture _fixture;
    private readonly Uri _uri;
    private readonly ApiHelper _apiHelper;
    private const int RunTimes = 100;

    public CoinTests()
    {
        _uri = new Uri("http://localHost:5555/coin", UriKind.Absolute);
        _client = new RestClient();
        _fixture = new Fixture();
        _apiHelper = new ApiHelper();
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
        Guid.TryParse(response.Content, out _).Should().BeTrue();
        await _apiHelper.Delete(_uri, coin.Id);
    }

    [Theory]
    [InlineData("", 1)]
    [InlineData("Silver", 0)]
    public async Task ShouldReturnBadWhenInvalidPassed(string name, decimal inGold)
    {
        var coin = new Coin { Name = name, InGold = inGold };

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
        await _apiHelper.Insert(_uri, coin);

        var request = new RestRequest(_uri)
            .AddJsonBody(coin);

        var response = await _client.ExecutePostAsync(request);
        response.IsSuccessful.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        await _apiHelper.Delete(_uri, coin.Id);
    }

    //PUt
    [Fact]
    public async Task WhenNotExistsShouldFailCreateWhenInvalid()
    {
        foreach (var coin in _fixture.CreateMany<Coin>(RunTimes))
        {
            coin.Name = "";

            var request = new RestRequest(_uri)
                .AddJsonBody(coin);

            var response = await _client.ExecutePutAsync(request);
            response.IsSuccessful.Should().BeFalse();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }

    [Fact]
    public async Task WhenNotExistsShouldFailCreateWhenInvalidInGold()
    {
        foreach (var coin in _fixture.CreateMany<Coin>(RunTimes))
        {
            coin.InGold = 0;

            var request = new RestRequest(_uri)
                .AddJsonBody(coin);

            var response = await _client.ExecutePutAsync(request);
            response.IsSuccessful.Should().BeFalse();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }

    [Fact]
    public async Task WhenNameTakenAndExistsShouldBadRequest()
    {
        var coins = _fixture.CreateMany<Coin>().ToList();
        await _apiHelper.Insert(_uri, coins);

        var coin = new Coin { Id = coins.First().Id, Name = coins.Last().Name, InGold = 10M };
        var request = new RestRequest(_uri)
            .AddJsonBody(coin);

        var response = await _client.ExecutePutAsync(request);
        response.IsSuccessful.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        response.Content.Should().Contain("Name");
        await _apiHelper.Delete(_uri, coin.Id);
    }

    [Fact]
    public async Task WhenNameTakenAndNotExistsShouldBadRequest()
    {
        var original = _fixture.Create<Coin>();
        await _apiHelper.Insert(_uri, original);

        var coin = new Coin { Id = _fixture.Create<Guid>(), Name = original.Name, InGold = 10M };
        var request = new RestRequest(_uri)
            .AddJsonBody(coin);

        var response = await _client.ExecutePutAsync(request);
        response.IsSuccessful.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        response.Content.Should().Contain("Name");
        await _apiHelper.Delete(_uri, original.Id);
    }

    [Fact]
    public async Task ShouldBeAbleToUpdateInGold()
    {
        var original = _fixture.Create<Coin>();
        await _apiHelper.Insert(_uri, original);

        foreach (var inGold in _fixture.CreateMany<decimal>(RunTimes))
        {
            var coin = new Coin { Id = original.Id, InGold = inGold };
            var response = await _apiHelper.Update(_uri, coin);
            response.IsSuccessful.Should().BeTrue();
        }
        await _apiHelper.Delete(_uri, original.Id);
    }

    [Fact]
    public async Task ShouldBeAbleToUpdateName()
    {
        var original = _fixture.Create<Coin>();
        await _apiHelper.Insert(_uri, original);

        foreach (var name in _fixture.CreateMany<string>(RunTimes))
        {
            var coin = new Coin { Id = original.Id, Name = name };
            var response = await _apiHelper.Update(_uri, coin);
            response.IsSuccessful.Should().BeTrue();
        }
        await _apiHelper.Delete(_uri, original.Id);
    }

    [Fact]
    public async Task ShouldNotUpdateDefaultValueName()
    {
        var original = _fixture.Create<Coin>();
        await _apiHelper.Insert(_uri, original);

        var coin = new Coin { Id = original.Id, Name = "", InGold = 10M };
        var request = new RestRequest(_uri)
            .AddJsonBody(coin);

        var response = await _client.PutAsync(request);
        response.IsSuccessful.Should().BeTrue();

        var result = await _apiHelper.GetById<Coin>(_uri, original.Id);
        result.Id.Should().Be(original.Id);
        result.Name.Should().BeEquivalentTo(original.Name);
        result.InGold.Should().NotBe(original.InGold);
        result.InGold.Should().Be(coin.InGold);
        await _apiHelper.Delete(_uri, original.Id);
    }

    [Fact]
    public async Task ShouldNotUpdateDefaultValueInGold()
    {
        var original = _fixture.Create<Coin>();
        await _apiHelper.Insert(_uri, original);

        var coin = new Coin { Id = original.Id, Name = _fixture.Create<string>(), InGold = 0 };
        var request = new RestRequest(_uri)
            .AddJsonBody(coin);

        var response = await _client.ExecutePutAsync(request);
        response.IsSuccessful.Should().BeTrue();

        var result = await _apiHelper.GetById<Coin>(_uri, original.Id);
        result.Id.Should().Be(original.Id);
        result.InGold.Should().Be(original.InGold);
        result.Name.Should().NotBeEquivalentTo(original.Name);
        result.Name.Should().BeEquivalentTo(coin.Name);
        await _apiHelper.Delete(_uri, original.Id);
    }

    [Fact]
    public async Task ShouldCreateWhenNotExists()
    {
        var coin = _fixture.Create<Coin>();

        var request = new RestRequest(_uri)
            .AddJsonBody(coin);

        var response = await _client.ExecutePutAsync(request);
        response.IsSuccessful.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        Guid.TryParse(response.Content, out _).Should().BeTrue();
        await _apiHelper.Delete(_uri, coin.Id);
    }

    // Get
    [Fact]
    public async Task ShouldReturnCoinByName()
    {
        var coins = _fixture.CreateMany<Coin>(RunTimes).ToList();
        await _apiHelper.Insert(_uri, coins);

        foreach (var expected in coins)
        {
            var actual = await GetByName(expected);
            actual.Should().BeEquivalentTo(expected);
            await _apiHelper.Delete(_uri, expected.Id);
        }
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
        var coins = _fixture.CreateMany<Coin>(RunTimes).ToList();
        await _apiHelper.Insert(_uri, coins);

        foreach (var expected in coins)
        {
            var actual = await _apiHelper.GetById<Coin>(_uri, expected.Id);
            actual.Should().BeEquivalentTo(expected);
            await _apiHelper.Delete(_uri, expected.Id);
        }
    }

    [Fact]
    public async Task ShouldReturnAll()
    {
        var coins = _fixture.CreateMany<Coin>(10).ToList();
        await _apiHelper.Insert(_uri, coins);

        var request = new RestRequest(_uri);
        var response = await _client.GetAsync(request);
        var actual = JsonConvert.DeserializeObject<IEnumerable<Coin>>(response.Content).ToList();

        foreach (var coin in coins)
        {
            actual.Should().ContainEquivalentOf(coin);
            await _apiHelper.Delete(_uri, coin.Id);
        }
    }

    //Delete 
    [Fact]
    public async Task ShouldDeleteWHenFound()
    {
        var coins = _fixture.CreateMany<Coin>(RunTimes).ToList();
        await _apiHelper.Insert(_uri, coins);

        foreach (var coin in coins)
        {
            var request = new RestRequest($"{_uri}?id={coin.Id}");
            var response = await _client.DeleteAsync(request);
            response.IsSuccessful.Should().BeTrue();
        }
    }

    [Fact]
    public async Task ShouldGetNotFound()
    {
        var request = new RestRequest($"{_uri}?id={Guid.Empty}");
        var response = await _client.DeleteAsync(request);
        response.IsSuccessful.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    private async Task<Coin> GetByName(Coin expected)
    {
        var request = new RestRequest($"{_uri}?name={expected.Name}");
        var response = await _client.GetAsync(request);
        var actual = JsonConvert.DeserializeObject<Coin>(response.Content);
        return actual;
    }
}