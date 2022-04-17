using System;
using System.Collections.Generic;
using System.Linq;
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
    private const int RunCount = 1;
    
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
            await _apiHelper.Delete(_rollerUri, coinRoller.Coin.Id);
            await _apiHelper.Delete(_coinUri, coinRoller.Id);
        }
    }

    private async Task Insert(CoinRoller roller)
    {
        await _apiHelper.Insert(_coinUri, roller.Coin);
        await _apiHelper.Insert(_rollerUri, roller);
    }

    private async Task Insert(IEnumerable<CoinRoller> rollers)
    {
        await _apiHelper.Insert(_coinUri, rollers.Select(x => x.Coin));
        await _apiHelper.Insert(_rollerUri, rollers);
    }
}