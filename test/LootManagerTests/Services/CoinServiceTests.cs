using FluentAssertions;
using Manager.Models;
using Manager.Services;
using Xunit;

namespace LootManagerTests.Services;

public class CoinServiceTests
{
    private readonly CoinService _sut;

    public CoinServiceTests()
    {
        _sut = new CoinService();
    }
    
    [Fact]
    public void ShouldReturnCopperTl1RollBelow15()
    {
        var result = _sut.Get(1, 14);
        result.InGold.Should().Be(Coins.Copper.InGold);
    } 
}