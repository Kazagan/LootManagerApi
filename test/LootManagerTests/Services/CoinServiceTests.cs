using FluentAssertions;
using Manager.Enums;
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
    public void ShouldReturnPlatGreaterThan95()
    {
        var result = _sut.Get(1, 96);
        result.Name.Should().Be(CoinType.Platinum);
    }

    [Fact]
    public void ShouldBeWithinCopperRange()
    {
        var result = _sut.Get(1, 14);
        result.Count.Should().BeInRange(2000, 12000);
    }

    [Fact]
    public void ShouldBeWithinNickleRange()
    {
        var result = _sut.Get(1, 15);
        result.Count.Should().BeInRange(400, 2400);
    }

    [Fact]
    public void ShouldBeWithinSilverRange()
    {
        var result = _sut.Get(1, 52);
        result.Count.Should().BeInRange(200, 1200);
    }

    [Fact]
    public void ShouldBeWithinGoldRange()
    {
        var result = _sut.Get(1, 95);
        result.Count.Should().BeInRange(20, 160);
    }

    [Fact]
    public void ShouldBeWithinPlatRange()
    {
        var result = _sut.Get(1, 100);
        result.Count.Should().BeInRange(10, 40);
    }
}