using Data.Models;
using FluentAssertions;
using Manager.Services;
using Xunit;

namespace LootManagerTests.Services;

public class RollerTests
{
    private readonly Roller _sut;

    public RollerTests()
    {
        _sut = new Roller();
    }

    [Fact]
    public void RollShouldReturnRollsForItems()
    {
        var result = _sut.Roll(1);
        result.Cash.Should().NotBe(null);
        result.Gems.Should().NotBe(null);
        result.Items.Should().NotBe(null);
    }

    [Fact]
    public void RollShouldReturnCoin()
    {
        var result = _sut.Roll(1);
        result.Cash.Should().BeOfType<Coin>();
    }
}