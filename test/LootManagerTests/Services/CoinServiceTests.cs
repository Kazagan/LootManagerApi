using System;
using FluentAssertions;
using Manager.Enums;
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
    public void ShouldReturnCopperLessThan15()
    {
        var result = _sut.Get(1, 14);
        result.Name.Should().Be(CoinType.Copper);
    }
    [Fact]
    public void ShouldReturnNickelLessThan30()
    {
        var result = _sut.Get(1, 29);
        result.Name.Should().Be(CoinType.Nickel);
    }
    [Fact]
    public void ShouldReturnSilverLessThan53()
    {
        var result = _sut.Get(1, 52);
        result.Name.Should().Be(CoinType.Silver);
    }
    [Fact]
    public void ShouldReturnGoldLessThan96()
    {
        var result = _sut.Get(1, 95);
        result.Name.Should().Be(CoinType.Gold);
    }
    [Fact]
    public void ShouldReturnPlatGreaterThan65()
    {
        var result = _sut.Get(1, 96);
        result.Name.Should().Be(CoinType.Platinum);
    }
}