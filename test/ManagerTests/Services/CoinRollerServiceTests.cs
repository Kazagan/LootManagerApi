using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
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
        _sut.GetAll().Should().BeEquivalentTo(rollers);
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
        var rollers = _fixture.Build<CoinRoller>()
            .With(x => x.TreasureLevel, 1)
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
        _sut.Get(1, testRollMin).Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void ShouldCallInsertOnExpected()
    {
        var roller = _fixture.Create<CoinRoller>();
        CoinRoller? inserted = null;
        _repository.Setup(x => x.Insert(It.IsAny<CoinRoller>()))
            .Callback<CoinRoller>(x => inserted = x )
            .Verifiable();
        _repository.Setup(x => x.Get<Coin>(roller.Coin.Id))
            .Returns(roller.Coin);
        _repository.Setup(x => x.Save())
            .Verifiable();
        _sut.Create(roller);
        inserted.Should().BeEquivalentTo(roller);
    }

    [Fact]
    public void IfRollerExistsShouldNotInsert()
    {
        var coinRoller = _fixture.CreateMany<CoinRoller>().ToList();
        SetUpMock(coinRoller);

        _sut.Create(coinRoller.First());
        _repository
            .Verify(x => x.Insert(It.IsAny<CoinRoller>()), Times.Never);
    }
    
    [Fact]
    public void IfNoCoinShouldNotInsert()
    {
        var coinRoller = _fixture.CreateMany<CoinRoller>().ToList();

        _sut.Create(coinRoller.First());
        _repository
            .Verify(x => x.Insert(It.IsAny<CoinRoller>()), Times.Never);
    }

    [Fact]
    public void ShouldNotInsertIfAllValuesNotProvided()
    {
        var roller = _fixture.Create<CoinRoller>();
        roller.Multiplier = 0;
        _sut.Create(roller);
        _repository.Verify(x => x.Insert(It.IsAny<CoinRoller>()), Times.Never);
    }
    
    //Update
    [Fact]
    public void ShouldNotUpdateWhenValidRollerPassed()
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
        
        _sut.Update(sample);
        callback.Should().BeEquivalentTo(sample);
    }
    
    [Fact]
    public void ShouldNotUpdateWhenNewCoinNotFoundPassed()
    {
        var rollers = _fixture.CreateMany<CoinRoller>(10).ToList();
        SetUpMock(rollers);
        var sample = _fixture.Create<CoinRoller>();

        sample.Id = rollers.First().Id;
        
        CoinRoller? callback = null;
        _repository
            .Setup(x => x.Update(It.IsAny<CoinRoller>()))
            .Callback<CoinRoller>(x => callback = x);
        
        var result = _sut.Update(sample);
        callback.Should().BeNull();
        result.Should().Contain("not found");
    }
    
    [Fact]
    public void ShouldNotUpdateWhenOriginalNotFound()
    {
        var rollers = _fixture.CreateMany<CoinRoller>(10).ToList();
        var sample = CreateCopy(rollers.First());
        sample.Multiplier = 10;
        
        CoinRoller? callback = null;
        _repository
            .Setup(x => x.Update(It.IsAny<CoinRoller>()))
            .Callback<CoinRoller>(x => callback = x);
        
        var result = _sut.Update(sample);
        result.Should().Be(Constants.NotFound);
        callback.Should().BeNull();
    }
    
    [Fact]
    public void ShouldUpdateCoinWhenChanged()
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

        _sut.Update(sample);
        callback.Should().BeEquivalentTo(sample);
    }
    
    
    [Fact]
    public void ShouldNotInsertIfRollsChangedAndAlreadyExist()
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
        
        var result = _sut.Update(sample);
        callback.Should().BeNull();
        result.Should().Be(Constants.Exists);
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
        SetUpMock(new List<CoinRoller> {roller});
    }
}