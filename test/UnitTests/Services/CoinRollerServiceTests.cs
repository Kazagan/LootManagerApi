using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Data.Entities;
using Data.Repositories;
using FluentAssertions;
using Manager;
using Manager.Services;
using Moq;
using Xunit;

namespace LootManagerTests.Services;

public class CoinRollerServiceTests
{
    private readonly Mock<IRepository> _repository;
    private readonly Fixture _fixture;
    private readonly CoinRollerService _sut;

    public CoinRollerServiceTests()
    {
        _fixture = new Fixture();
        _repository = new Mock<IRepository>();
        _sut = new CoinRollerService(_repository.Object);
    }

    [Fact]
    public void ShouldReturnAllWhenCalled()
    {
        var rollers = _fixture.CreateMany<CoinRoller>(100).ToList();
        SetUpMock(rollers);
        _sut.Get().Should().BeEquivalentTo(rollers);
    }

    [Fact]
    public void ShouldReturnForGivenId()
    {
        var roller = _fixture.Create<CoinRoller>();
        SetUpMock(roller);
        _sut.Get(roller.Id).Should().BeEquivalentTo(roller);
    }

    [Fact]
    public void ShouldReturnForGivenTreasureLevelAndRollMin()
    {
        const int treasureLevel = 1; // Manually set to avoid test passing because of single roll for treasure level.
        var rollers = _fixture.Build<CoinRoller>()
            .With(x => x.TreasureLevel, treasureLevel)
            .CreateMany(10).ToList();
        var level = 5;
        foreach (var roller in rollers)
        {
            roller.RollMin = level;
            level += 10;
        }
        SetUpMock(rollers);

        var testRollMin = 50;
        var expected = rollers.OrderBy(x => x.RollMin)
            .Last(x => x.RollMin < testRollMin);
        _sut.Get(treasureLevel, testRollMin).Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void ShouldReturnAllWithinTreasureLevel()
    {
        var level1 = _fixture.Build<CoinRoller>()
            .With(x => x.TreasureLevel, 1)
            .CreateMany(10).ToList();
        var level2 = _fixture.Build<CoinRoller>()
            .With(x => x.TreasureLevel, 2)
            .CreateMany(10).ToList();
        SetUpMock(level1.Concat(level2));

        var results = _sut.Get(treasureLevel: 1).ToList();
        results.Should().BeEquivalentTo(level1);
        results.Should().NotBeEquivalentTo(level2);
    }

    [Fact]
    public async Task ShouldCallInsertOnExpected()
    {
        var roller = _fixture.Create<CoinRoller>();
        CoinRoller? inserted = null;
        _repository.Setup(x => x.Insert(It.IsAny<CoinRoller>()))
            .Callback<CoinRoller>(x => inserted = x);
        _repository.Setup(x => x.Get<Coin>(roller.Coin.Id))
            .Returns(roller.Coin);
        _repository.Setup(x => x.Save())
            .Verifiable();
        await _sut.Create(roller);
        inserted.Should().BeEquivalentTo(roller);
    }

    [Fact]
    public async Task ShouldReturnIdWhenCreated()
    {
        var roller = _fixture.Create<CoinRoller>();
        SetUpMock(roller.Coin);

        var result = await _sut.Create(roller);
        Guid.TryParse(result, out var id).Should().BeTrue();
    }

    [Fact]
    public async Task IfRollerExistsShouldNotInsert()
    {
        var coinRoller = _fixture.CreateMany<CoinRoller>().ToList();
        SetUpMock(coinRoller);

        await _sut.Create(coinRoller.First());
        _repository
            .Verify(x => x.Insert(It.IsAny<CoinRoller>()), Times.Never);
    }

    [Fact]
    public async Task IfNoCoinShouldNotInsert()
    {
        var coinRoller = _fixture.CreateMany<CoinRoller>().ToList();

        await _sut.Create(coinRoller.First());
        _repository
            .Verify(x => x.Insert(It.IsAny<CoinRoller>()), Times.Never);
    }

    [Fact]
    public async Task ShouldNotInsertIfAllValuesNotProvided()
    {
        var roller = _fixture.Create<CoinRoller>();
        roller.Multiplier = 0;
        await _sut.Create(roller);
        _repository.Verify(x => x.Insert(It.IsAny<CoinRoller>()), Times.Never);
    }

    //Update
    [Fact]
    public async Task ShouldNotUpdateWhenValidRollerPassed()
    {
        var rollers = _fixture.CreateMany<CoinRoller>(10).ToList();
        SetUpMock(rollers);
        var sample = _fixture.Create<CoinRoller>();
        sample.Id = rollers.First().Id;
        SetUpMock(sample.Coin);

        CoinRoller? callback = null;
        _repository
            .Setup(x => x.Update(It.IsAny<CoinRoller>()))
            .Callback<CoinRoller>(x => callback = x);

        await _sut.Update(sample);
        callback.Should().BeEquivalentTo(sample);
    }

    [Fact]
    public async Task ShouldNotUpdateWhenNewCoinNotFoundPassed()
    {
        var rollers = _fixture.CreateMany<CoinRoller>(10).ToList();
        SetUpMock(rollers);
        var sample = _fixture.Create<CoinRoller>();

        sample.Id = rollers.First().Id;

        CoinRoller? callback = null;
        _repository
            .Setup(x => x.Update(It.IsAny<CoinRoller>()))
            .Callback<CoinRoller>(x => callback = x);

        var result = await _sut.Update(sample);
        callback.Should().BeNull();
        result.Should().Be(Constants.Invalid);
    }

    [Fact]
    public async Task ShouldNotUpdateWhenOriginalNotFound()
    {
        var rollers = _fixture.CreateMany<CoinRoller>(10).ToList();
        var sample = CreateCopy(rollers.First());
        sample.Multiplier = 10;

        CoinRoller? callback = null;
        _repository
            .Setup(x => x.Update(It.IsAny<CoinRoller>()))
            .Callback<CoinRoller>(x => callback = x);

        var result = await _sut.Update(sample);
        result.Should().Be(Constants.NotFound);
        callback.Should().BeNull();
    }

    [Fact]
    public async Task ShouldUpdateCoinWhenChanged()
    {
        var rollers = _fixture.CreateMany<CoinRoller>(10).ToList();
        SetUpMock(rollers);
        var sample = CreateCopy(rollers.First());
        sample.Coin = _fixture.Create<Coin>();
        SetUpMock(sample.Coin);

        CoinRoller? callback = null;
        _repository
            .Setup(x => x.Update(It.IsAny<CoinRoller>()))
            .Callback<CoinRoller>(x => callback = x);

        await _sut.Update(sample);
        callback.Should().BeEquivalentTo(sample);
    }


    [Fact]
    public async Task ShouldNotInsertIfRollsChangedAndAlreadyExist()
    {
        var rollers = _fixture.CreateMany<CoinRoller>(10).ToList();
        SetUpMock(rollers);
        var sample = CreateCopy(rollers.First());

        sample.TreasureLevel = rollers.Last().TreasureLevel;
        sample.RollMin = rollers.Last().RollMin;

        CoinRoller? callback = null;
        _repository
            .Setup(x => x.Update(It.IsAny<CoinRoller>()))
            .Callback<CoinRoller>(x => callback = x);

        var result = await _sut.Update(sample);
        callback.Should().BeNull();
        result.Should().Be(Constants.Exists);
    }

    //DELETE
    [Fact]
    public async Task ShouldCallDeleteForExpectdeCoinWhenIdFound()
    {
        var roller = _fixture.Create<CoinRoller>();
        SetUpMock(roller);

        await _sut.Delete(roller.Id);
        _repository.Verify(x => x.Delete(It.IsAny<CoinRoller>()), Times.Once);
    }

    //DELETE
    [Fact]
    public async Task ShouldNotCallDeleteForExpectedCoinWhenNotFound()
    {
        var roller = _fixture.Create<CoinRoller>();

        await _sut.Delete(roller.Id);
        _repository.Verify(x => x.Delete(It.IsAny<CoinRoller>()), Times.Never);
    }

    private CoinRoller CreateCopy(CoinRoller roller)
    {
        return new CoinRoller
        {
            Id = roller.Id,
            TreasureLevel = roller.TreasureLevel,
            RollMin = roller.RollMin,
            Coin = roller.Coin,
            DiceCount = roller.DiceCount,
            DiceSides = roller.DiceSides,
            Multiplier = roller.Multiplier
        };
    }

    private void SetUpMock(IEnumerable<CoinRoller> rollers)
    {
        _repository
            .Setup(x => x.Get<CoinRoller>())
            .Returns(rollers.AsQueryable());
    }

    private void SetUpMock(Coin coin)
    {
        _repository
            .Setup(x => x.Get<Coin>(It.IsAny<Guid>()))
            .Returns(coin);
    }
    private void SetUpMock(CoinRoller roller)
    {
        SetUpMock(new List<CoinRoller> { roller });
    }
}