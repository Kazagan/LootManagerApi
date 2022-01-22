using System.Linq;
using AutoFixture;
using Data.Models;
using Data.Repositories;
using FluentAssertions;
using Manager.Services;
using Moq;
using Xunit;

namespace LootManagerTests.Services;

public class GoodServiceTest
{
    private readonly GoodService _sut;
    private readonly Mock<IRepository<ManagerContext>> _repository;
    private readonly Fixture _fixture;

    public GoodServiceTest()
    {
        _fixture = new Fixture();
        _repository = new Mock<IRepository<ManagerContext>>();
        _sut = new GoodService(_repository.Object);
    }

    [Fact]
    public void GetTypeShouldReturnExpectedTypes()
    {
        var goodTypes = _fixture.CreateMany<GoodTypeRoller>();
        var sample = goodTypes.First();

        _repository
            .Setup(x => x.Get<GoodTypeRoller>())
            .Returns(goodTypes.AsQueryable);

        var result = _sut.Get(sample.TreasureLevel, sample.RollMin);
        result.Type.Should().Be(sample.Type);
    }

    [Fact]
    public void ShouldSetValue()
    {
        var goodTypes = _fixture
            .Build<GoodTypeRoller>()
            .With(x => x.RollMax, 100)
            .With(x => x.RollMin, 1)
            .CreateMany();
        var sampleType = goodTypes.First();
        
        var goodRoller = _fixture
            .Build<GoodRoller>()
            .With(x => x.Good, _fixture
                .Build<Good>()
                .With(x => x.Type, sampleType.Type)
                .Create)
            .With(x => x.RollMin, 1)
            .With(x => x.RollMax, 100)
            .CreateMany()
            .ToList();
        var goodSample = goodRoller.First();

        var goods = goodRoller.Select(x => x.Good);
        
        _repository
            .Setup(x => x.Get<GoodTypeRoller>())
            .Returns(goodTypes.AsQueryable);
        _repository
            .Setup(x => x.Get<GoodRoller>())
            .Returns(goodRoller.AsQueryable);
        _repository
            .Setup(x => x.Get<Good>())
            .Returns(goods.AsQueryable);

        var result = _sut.Get(sampleType.TreasureLevel, sampleType.RollMin);
        var min = 1 * goodSample.DiceCount * goodSample.Multiplier;
        var max = goodSample.DiceSides * goodSample.DiceCount * goodSample.Multiplier;
        result.Value.Count.Should().BeInRange(min, max);
    }

    [Fact]
    public void ShouldReturnDefaultGoodWhenNoResult()
    {
        var result = _sut.Get(1, 15);
        result.Should().BeEquivalentTo(new Good());
    }
}