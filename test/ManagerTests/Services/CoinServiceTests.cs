using System;
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
    private readonly Mock<IRepository> _repository;
    private readonly Fixture _fixture;

    public CoinServiceTests()
    {
        _fixture = new Fixture();
        _repository = new Mock<IRepository>();
        _sut = new CoinService(_repository.Object);
    }

    [Fact]
    public void ShouldReturnAll()
    {
        var coins = _fixture.CreateMany<Coin>(100).ToList();
        SetupRepoMock(coins);

        var result = _sut.ReadAll();
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
        var sample = _fixture.Create<Coin>();
        SetupRepoMock(sample);

        sample.Name = "newName";
        var result = _sut.Update(sample);
        result.Name.Should().Be(sample.Name);
    }
    
    [Fact]
    public void ShouldCallUpdateWithExpectedNewInGold()
    {
        var coin = _fixture.Create<Coin>();
        SetupRepoMock(coin);

        coin.InGold = 10;
        var result = _sut.Update(coin);
        result.InGold.Should().Be(coin.InGold);
    }

    [Fact]
    public void ShouldReturnCoinIfIdExists()
    {
        var coin = _fixture.Create<Coin>();
        SetupRepoMock(coin);
        var result = _sut.Read(coin.Id);
        result.Should().BeEquivalentTo(coin);
    }
    
    [Fact]
    public void ShouldReturnCoinIfNameExists()
    {
        var coins = _fixture.CreateMany<Coin>().ToList();
        SetupRepoMock(coins);
        var coin = coins.First();
        var result = _sut.Read(coin.Name);
        result.Should().BeEquivalentTo(coin);
    }
    
    [Fact]
    public void ShouldReturnCoinIfIdDoesntExists()
    {
        var coin = _fixture.Create<Coin>();
        SetupRepoMock(coin);
        var result = _sut.Read(new Guid());
        result.Should().BeNull();
    }
    
    [Fact]
    public void ShouldReturnCoinIfNameDoesntExists()
    {
        var coins = _fixture.Create<Coin>();
        SetupRepoMock(coins);
        var result = _sut.Read("");
        result.Should().BeNull();
    }

    [Fact]
    public void ShouldUpdateBothWhenBothChanged()
    {
        var coin = _fixture.Create<Coin>();

        SetupRepoMock(coin);

        coin.InGold = 10;
        coin.Name = "new name";
        var result = _sut.Update(coin);
        result.InGold.Should().Be(coin.InGold);
        result.Name.Should().Be(coin.Name);
    }

    [Fact]
    public void ShouldNotChangeIfNullPassed()
    {
        var coin = _fixture.Create<Coin>();

        SetupRepoMock(coin);
        coin.Name = "";
        coin.InGold = 0;
        var result = _sut.Update(coin);
        coin.Should().BeEquivalentTo(result);
    }

    [Fact]
    public void ShouldCallDeleteForExpectedCoinWhenIdFound()
    {
        var coin = _fixture.Create<Coin>();
        SetupRepoMock(coin);

        _sut.Delete(coin.Id);
        _repository
            .Verify(x => x.Delete(It.IsAny<Coin>()), Times.Once);
    }

    [Fact]
    public void ShouldNotCallDeleteWhenNotFound()
    {
        var coins = _fixture.Create<Coin>();
        SetupRepoMock(coins);
        _sut.Delete(new Guid());
        _repository
            .Verify(x => x.Delete(It.IsAny<Coin>()), Times.Never);
    }

    private void SetupRepoMock(IEnumerable<Coin> coins)
    {
        _repository
            .Setup(x => x.Get<Coin>())
            .Returns(coins.AsQueryable);
    }
    private void SetupRepoMock(Coin coin)
    {
        _repository
            .Setup(x => x.Get<Coin>(coin.Id))
            .Returns(coin);
    }
}