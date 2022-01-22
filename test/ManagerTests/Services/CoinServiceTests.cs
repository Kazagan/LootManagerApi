using System.Linq;
using AutoFixture;
using Data.Enums;
using Data.Models;
using Data.Repositories;
using FluentAssertions;
using Manager.Services;
using Moq;
using Xunit;

namespace LootManagerTests.Services;

public class CoinServiceTests
{
    private readonly CoinService _sut;
    private readonly Mock<IRepository<ManagerContext>> _repository;
    private readonly Fixture _fixture;

    public CoinServiceTests()
    {
        _fixture = new Fixture();
        _repository = new Mock<IRepository<ManagerContext>>();
        _sut = new CoinService(_repository.Object);
    }

    [Fact]
    public void ShouldReturnExpectedCoinTypeForTreasureLevelAndRoll()
    {
        var coinTables = _fixture.CreateMany<CoinRoller>().ToList();
        _repository
            .Setup(x => x.Get<CoinRoller>())
            .Returns(coinTables.AsQueryable);

        var sampleCoin = coinTables.First();

        var result = _sut.Get(sampleCoin.TreasureLevel, sampleCoin.RollMin);
        result?.CoinType.Should().Be(sampleCoin.Coin.CoinType);
    }

    [Fact]
    public void ShouldReturnCountWithinRange()
    {
        var coinTables = _fixture
            .Build<CoinRoller>()
            .With(x => x.Coin, _fixture.Create<Coin>())
            .With(x => x.RollMax, 100)
            .With(x => x.RollMin, 1)
            .CreateMany().ToList();
        _repository
            .Setup(x => x.Get<CoinRoller>())
            .Returns(coinTables.AsQueryable);

        var sampleCoin = coinTables.First();
        var result = _sut.Get(sampleCoin.TreasureLevel, sampleCoin.RollMin);
        var min = 1 * sampleCoin.DiceCount * sampleCoin.Multiplier;
        var max = sampleCoin.DiceCount * sampleCoin.DiceSides * sampleCoin.Multiplier;
        result.Count.Should().BeInRange(min, max);
    }

    [Fact]
    public void ShouldReturnDefaultIfNoMatch()
    {
        var result = _sut.Get(1, 15);
        result.Id.Should().Be(0);
    }
}