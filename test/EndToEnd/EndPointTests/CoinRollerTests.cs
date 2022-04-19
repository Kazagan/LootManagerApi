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

public class CoinRollerTests
{
    private readonly RestClient _client;
    private readonly Fixture _fixture;
    private readonly Uri _rollerUri;
    private readonly Uri _coinUri;
    private readonly ApiHelper _apiHelper;
    private const int RunCount = 100;
    
    public CoinRollerTests()
    {
        var uriString = "http://localHost:5555";
        _rollerUri = new Uri(uriString + "/coinRoller", UriKind.Absolute);
        _coinUri = new Uri(uriString + "/Coin", UriKind.Absolute);
        _client = new RestClient();
        _fixture = new Fixture();
        _apiHelper = new ApiHelper();
    }
    
    // GET
    [Fact]
    public async Task ShouldGetAll()
    {
        var coinRollers = _fixture.CreateMany<CoinRoller>(10).ToList();
        await Insert(coinRollers);

        var request = new RestRequest(_rollerUri);
        var response = await _client.GetAsync(request);
        response.IsSuccessful.Should().BeTrue();
        var actual = JsonConvert.DeserializeObject<IEnumerable<CoinRoller>>(response.Content).ToList();
        foreach (var coinRoller in coinRollers)
        {
            actual.Should().ContainEquivalentOf(coinRoller);
            await Delete(coinRoller);
        }
    }

    [Fact]
    public async Task ShouldGetById()
    {
        var rollers = _fixture.CreateMany<CoinRoller>(RunCount).ToList();
        await Insert(rollers);

        foreach (var expectation in rollers)
        {
            var request = new RestRequest($"{_rollerUri}/{expectation.Id}");
            var response = await _client.GetAsync(request);
            var result = JsonConvert.DeserializeObject<CoinRoller>(response.Content);
            result.Should().BeEquivalentTo(expectation);
            await Delete(expectation);
        }
    }
    
    [Fact]
    public async Task ShouldGetByTreasureLevelAndRoll()
    {
        var rollers = _fixture.CreateMany<CoinRoller>(RunCount).ToList();
        await Insert(rollers);

        var expected = rollers.First();

        var request = new RestRequest(_rollerUri)
            .AddParameter("treasureLevel", expected.TreasureLevel)
            .AddParameter("roll", expected.RollMin + 1); // Add One to surpass min by minimum amount

        var response = await _client.GetAsync(request);
        var result = JsonConvert.DeserializeObject<CoinRoller>(response.Content);
        result.Should().BeEquivalentTo(expected);
        await Delete(rollers);
    }
    
    [Fact]
    public async Task ShouldGetAllByTreasureLevel()
    {
        var rollers = _fixture.CreateMany<CoinRoller>(RunCount).ToList();
        await Insert(rollers);

        var expected = rollers.First();
        var request = new RestRequest(_rollerUri)
            .AddParameter("treasureLevel", expected.TreasureLevel);

        var response = await _client.GetAsync(request);
        var result = JsonConvert.DeserializeObject<IEnumerable<CoinRoller>>(response.Content).ToList();
        foreach (var expectation in rollers.Where(x => x.TreasureLevel == expected.TreasureLevel))
        {
            result.Should().ContainEquivalentOf(expectation);
        }
        
        await Delete(rollers);
    }

    [Fact]
    public async Task ShouldReturnNotFoundWhenSearchedForRoll()
    {
        var request = new RestRequest(_rollerUri)
            .AddParameter("treasureLevel", 1)
            .AddParameter("roll", 1);

        var response = await _client.GetAsync(request);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task ShouldReturnNotFoundWhenSearchedTreasure()
    {
        var request = new RestRequest(_rollerUri)
            .AddParameter("treasureLevel", -1); // -1 remove chance for match

        var response = await _client.GetAsync(request);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task ShouldReturnNotFoundWhenSearchedForId()
    {
        var request = new RestRequest($"{_rollerUri}/{Guid.Empty}");

        var response = await _client.GetAsync(request);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    // POST
    [Fact]
    public async Task ShouldCreateAndReturnIdForValid()
    {
        var rollers = _fixture.CreateMany<CoinRoller>(RunCount).ToList();
        await _apiHelper.Insert(_coinUri, rollers.Select(x => x.Coin));

        foreach (var roller in rollers)
        {
            var request = new RestRequest(_rollerUri)
                .AddJsonBody(roller);
            var response = await _client.PostAsync(request);
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            Guid.TryParse(response.Content, out _).Should().BeTrue();
            await Delete(roller);
        }
    }

    [Fact]
    public async Task ShouldReturnExistsWhenTreasureLevelRollTaken()
    {
        var original = _fixture.Create<CoinRoller>();
        await Insert(original);

        var newRoller = _fixture.Create<CoinRoller>();
        newRoller.TreasureLevel = original.TreasureLevel;
        newRoller.RollMin = original.RollMin;

        var request = new RestRequest(_rollerUri)
            .AddJsonBody(newRoller);
        var response = await _client.ExecutePostAsync(request);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        response.Content.Should().Contain("Exists");
    }

    [Fact]
    public async Task ShouldReturnInvalidWhenPropertyIsDefault()
    {
        var badRoller = new CoinRoller();
        
        var request = new RestRequest(_rollerUri)
            .AddJsonBody(badRoller);
        var response = await _client.ExecutePostAsync(request);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        response.Content.Should().Contain("Invalid");
    }
    
    [Fact]
    public async Task ShouldReturnInvalidWhenGoodCoinButInvalidRoller()
    {
        var badRoller = new CoinRoller();
        var coin = _fixture.Create<Coin>();
        await _apiHelper.Insert(_coinUri, coin);
        badRoller.Coin = coin;
        
        var request = new RestRequest(_rollerUri)
            .AddJsonBody(badRoller);
        var response = await _client.ExecutePostAsync(request);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        response.Content.Should().Contain("Invalid");
    }
    
    // PUT

    [Fact]
    public async Task ShouldCreateNewWhenIdEmpty()
    {
        var roller = _fixture.Create<CoinRoller>();
        await _apiHelper.Insert(_coinUri, roller.Coin);
        roller.Id = Guid.Empty;
        
        var request = new RestRequest(_rollerUri)
            .AddJsonBody(roller);
        var response = await _client.ExecutePutAsync(request);
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        await Delete(roller);
    }
    
    [Fact]
    public async Task ShouldReturnNotFoundWhenIdDoesntExist()
    {
        var roller = _fixture.Create<CoinRoller>();
        
        var request = new RestRequest(_rollerUri)
            .AddJsonBody(roller);
        var response = await _client.ExecutePutAsync(request);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task ShouldReturnNadRequestWhenNewCoinDoesntExist()
    {
        var roller = _fixture.Create<CoinRoller>();
        await Insert(roller);
        roller.Coin = _fixture.Create<Coin>();
        
        var request = new RestRequest(_rollerUri)
            .AddJsonBody(roller);
        var response = await _client.ExecutePutAsync(request);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        await Delete(roller);
    }
    
    [Fact]
    public async Task ShouldReturnOkWhenSuccessfulChanges()
    {
        var roller = _fixture.Create<CoinRoller>();
        await Insert(roller);

        roller.Multiplier = _fixture.Create<int>();
        var request = new RestRequest(_rollerUri)
            .AddJsonBody(roller);
        var response = await _client.ExecutePutAsync(request);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        await Delete(roller);
    }
    
    [Fact]
    public async Task ShouldReturnOkWhenSuccessfulChangesCoin()
    {
        var roller = _fixture.Create<CoinRoller>();
        await Insert(roller);
        var coin = _fixture.Create<Coin>();
        await _apiHelper.Insert(_coinUri, coin);
        roller.Coin = coin;
        
        roller.Multiplier = _fixture.Create<int>();
        var request = new RestRequest(_rollerUri)
            .AddJsonBody(roller);
        var response = await _client.ExecutePutAsync(request);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        await Delete(roller);
    }

    [Fact]
    public async Task ShouldDeleteWhenFound()
    {
        var rollers = _fixture.CreateMany<CoinRoller>(RunCount).ToList();
        await Insert(rollers);

        foreach (var roller in rollers)
        {
            var request = new RestRequest(_rollerUri)
                .AddParameter("id", roller.Id);
            var response = await _client.DeleteAsync(request);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            await _apiHelper.Delete(_coinUri, roller.Coin.Id);
        }
    }
    
    [Fact]
    public async Task ShouldDeleteWhenNotFound()
    {
        var request = new RestRequest(_rollerUri)
            .AddParameter("id", _fixture.Create<Guid>());
        var response = await _client.DeleteAsync(request);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    private async Task Insert(CoinRoller roller)
    {
        await _apiHelper.Insert(_coinUri, roller.Coin);
        await _apiHelper.Insert(_rollerUri, roller);
    }

    private async Task Insert(IEnumerable<CoinRoller> rollers)
    {
        var coinRollers = rollers.ToList();
        await _apiHelper.Insert(_coinUri, coinRollers.Select(x => x.Coin));
        await _apiHelper.Insert(_rollerUri, coinRollers);
    }

    private async Task Delete(CoinRoller roller)
    {
        await _apiHelper.Delete(_rollerUri, roller.Id);
        await _apiHelper.Delete(_coinUri, roller.Coin.Id);
    }

    private async Task Delete(IEnumerable<CoinRoller> rollers)
    {
        foreach (var roller in rollers)
        {
            await Delete(roller);
        }
    }
}