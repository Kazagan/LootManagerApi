using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AutoFixture;
using Data.Entities;
using Data.Repositories;
using FluentAssertions;
using Manager.Services;
using Moq;
using Xunit;

namespace LootManagerTests.Services;

public class CoinRollerServiceTests
{
    private readonly Mock<IRepository<ManagerContext>> _repository;
    private readonly Fixture _fixture;
    private readonly CoinRollerService _sut;
    
    public CoinRollerServiceTests()
    {
        _fixture = new Fixture();
        _repository = new Mock<IRepository<ManagerContext>>();
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
        _sut.Create(roller);
        CoinRoller? inserted = null;
        _repository.Setup(x => x.Insert(It.IsAny<Coin>()))
            .Callback<CoinRoller>(x => inserted = x )
            .Verifiable();
        inserted.Should().BeEquivalentTo(roller);
        _repository.Setup(x => x.Save())
            .Verifiable();
    }

    private void SetUpMock(IEnumerable<CoinRoller> rollers)
    {
        _repository.Setup(x => x.Get<CoinRoller>())
            .Returns(rollers.AsQueryable());
    }
    private void SetUpMock(CoinRoller roller)
    {
        _repository.Setup(x => x.Get<CoinRoller>(roller.Id))
            .Returns(roller);
    }
}