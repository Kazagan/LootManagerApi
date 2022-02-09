using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using Data.Entities;
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
    public void ShouldReturnAll()
    {
        var coins = _fixture.CreateMany<Coin>(100).ToList();
        SetupRepoMock(coins);

        var result = _sut.GetAll();
        result.Should().BeEquivalentTo(coins);
    }

    [Fact]
    public void ShouldCallSave()
    {
        _sut.Create(_fixture.Create<Coin>());
        _repository.Verify(x => x.Save(), Times.Once);
    }

    [Fact]
    public void ShouldSaveWithExpectedNameAndValue()
    {
        var expected = new Coin {Name = "test", InGold = (decimal).01};
        Coin? actual = null;
        _repository
            .Setup(x => x.Insert(It.IsAny<Coin>()))
            .Callback<Coin>(x => actual = x);
        _sut.Create(expected);
        actual?.Name.Should().BeEquivalentTo(expected.Name);
        actual?.InGold.Should().Be(expected.InGold);
    }

    [Fact]
    public void ShouldCallUpdateWithExpectedNewName()
    {
        var coins = _fixture.CreateMany<Coin>(10).ToList();
        var sample = coins.First();

        SetupRepoMock(coins);

        sample.Name = "newName";
        var result = _sut.Update(sample);
        result.Name.Should().Be(sample.Name);
    }
    
    [Fact]
    public void ShouldCallUpdateWithExpectedNewInGold()
    {
        var coins = _fixture.CreateMany<Coin>(10).ToList();
        var sample = coins.First();

        SetupRepoMock(coins);

        sample.InGold = 10;
        var result = _sut.Update(sample);
        result.InGold.Should().Be(sample.InGold);
    }

    [Fact]
    public void ShouldReturnCoinIfIdExists()
    {
        var coins = _fixture.CreateMany<Coin>().ToList();
        SetupRepoMock(coins);
        var sample = coins.First();
        var result = _sut.Get(sample.Id);
        result.Should().BeEquivalentTo(sample);
    }
    
    [Fact]
    public void ShouldReturnCoinIfNameExists()
    {
        var coins = _fixture.CreateMany<Coin>().ToList();
        SetupRepoMock(coins);
        var sample = coins.First();
        var result = _sut.Get(sample.Name);
        result.Should().BeEquivalentTo(sample);
    }
    
    [Fact]
    public void ShouldReturnCoinIfIdDoesntExists()
    {
        var coins = _fixture.CreateMany<Coin>();
        SetupRepoMock(coins);
        var result = _sut.Get(-1);
        result.Should().BeNull();
    }
    
    [Fact]
    public void ShouldReturnCoinIfNameDoesntExists()
    {
        var coins = _fixture.CreateMany<Coin>();
        SetupRepoMock(coins);
        var result = _sut.Get("");
        result.Should().BeNull();
    }

    [Fact]
    public void ShouldUpdateBothWhenBothChanged()
    {
        var coins = _fixture.CreateMany<Coin>(10).ToList();
        var sample = coins.First();

        SetupRepoMock(coins);

        sample.InGold = 10;
        sample.Name = "new name";
        var result = _sut.Update(sample);
        result.InGold.Should().Be(sample.InGold);
        result.Name.Should().Be(sample.Name);
    }

    [Fact]
    public void ShouldNotChangeIfNullPassed()
    {
        var coins = _fixture.CreateMany<Coin>(10).ToList();
        var sample = coins.First();

        SetupRepoMock(coins);
        sample.Name = null;
        sample.InGold = null;
        var result = _sut.Update(sample);
        sample.Should().BeEquivalentTo(result);
    }

    [Fact]
    public void ShouldCallDeleteForExpectedCoinWhenIdFound()
    {
        var coins = _fixture.CreateMany<Coin>().ToList();
        SetupRepoMock(coins);
        var sample = coins.First();

        _sut.Delete(sample.Id);
        _repository
            .Verify(x => x.Delete(It.IsAny<Coin>()), Times.Once);
    }

    [Fact]
    public void ShouldNotCallDeleteWhenNotFound()
    {
        var coins = _fixture.CreateMany<Coin>();
        SetupRepoMock(coins);
        _repository
            .Verify(x => x.Delete(It.IsAny<Coin>()), Times.Never);

        _sut.Delete(0);
        _repository.VerifyAll();
    }

    private void SetupRepoMock(IEnumerable<Coin> coins)
    {
        _repository
            .Setup(x => x.Get<Coin>())
            .Returns(coins.AsQueryable);
    }   
}