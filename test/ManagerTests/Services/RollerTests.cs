using Data.Entities;
using Data.Repositories;
using FluentAssertions;
using Manager.Services;
using Moq;
using Xunit;

namespace LootManagerTests.Services;

public class RollerTests
{
    private readonly Roller _sut;
    private readonly CoinService _coinService;

    public RollerTests()
    {
        var mockRepo = new Mock<IRepository<ManagerContext>>();
        _coinService = new CoinService(mockRepo.Object);
        _sut = new Roller(_coinService);
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