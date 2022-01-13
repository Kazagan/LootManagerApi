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
    public void ShouldReturnNothingWhenTlBelow15()
    {
        var result = _sut.Get(1, 14);
        result.Should().Be(null);
    }
    
    [Fact]
    public void ShouldReturnCopperTl1RollAbove14()
    {
        var result = _sut.Get(1, 15);
        result.InGold.Should().Be(Coins.Copper.InGold);
    }

    [Fact]
    public void ShouldReturnSilverIfTl1RollAbove29()
    {
        var result = _sut.Get(1, 30);
        result.InGold.Should().Be(Coins.Silver.InGold);
    }

    [Fact]
    public void ShouldReturnGoldIfTl1RollAbove52()
    {
        var results = _sut.Get(1, 53);
        results.InGold.Should().Be(Coins.Gold.InGold);
    }
}