using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using Data.Entities;
using Data.Repositories;
using FluentAssertions;
using Manager;
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

    // Reads
    [Fact]
    public void ShouldReturnAll()
    {
        var coins = _fixture.CreateMany<Coin>(100).ToList();
        SetupRepoMock(coins);

        var result = _sut.Get();
        result.Should().BeEquivalentTo(coins);
    }

    [Fact]
    public void ShouldReturnCoinIfIdExists()
    {
        var coin = _fixture.Create<Coin>();
        SetupRepoMock(coin);
        var result = _sut.Get(coin.Id);
        result.Should().BeEquivalentTo(coin);
    }

    [Fact]
    public void ShouldReturnNullIfIdDoesntExists()
    {
        var coin = _fixture.Create<Coin>();
        SetupRepoMock(coin);
        var result = _sut.Get(new Guid());
        result.Should().BeNull();
    }

    [Fact]
    public void ShouldReturnNullIfNameDoesntExists()
    {
        var coins = _fixture.Create<Coin>();
        SetupRepoMock(coins);
        var result = _sut.Get("");
        result.Should().BeNull();
    }

    [Fact]
    public void ShouldReturnCoinIfNameExists()
    {
        var coins = _fixture.CreateMany<Coin>().ToList();
        SetupRepoMock(coins);
        var coin = coins.First();
        var result = _sut.Get(coin.Name);
        result.Should().BeEquivalentTo(coin);
    }

    //Reads
    [Fact]
    public void ShouldCallSave()
    {
        _sut.Create(_fixture.Create<Coin>());
        _repository.Verify(x => x.Save(), Times.Once);
    }

    [Fact]
    public void ShouldSaveWithExpectedNameAndValue()
    {
        var expected = _fixture.Create<Coin>();

        Coin? actual = null;
        _repository
            .Setup(x => x.Insert(It.IsAny<Coin>()))
            .Callback<Coin>(x => actual = x);
        _sut.Create(expected);

        actual?.Name.Should().Be(expected.Name);
        actual?.InGold.Should().Be(expected.InGold);
    }

    [Fact]
    public void ShouldNotSaveWhenNameTaken()
    {
        var expected = _fixture.CreateMany<Coin>(1).ToList();
        SetupRepoMock(expected);

        Coin? actual = null;
        _repository
            .Setup(x => x.Insert(It.IsAny<Coin>()))
            .Callback<Coin>(x => actual = x);
        _sut.Create(expected.First());

        actual.Should().BeNull();
    }

    [Theory]
    [InlineData("Gold", 0)]
    [InlineData("", 10)]
    public void ShouldNotSaveInvalidCoin(string name, int inGold)
    {
        var coin = new Coin { Id = _fixture.Create<Guid>(), Name = name, InGold = inGold };
        _sut.Create(coin);
        _repository.Verify(x => x.Save(), Times.Never);
    }

    // Update
    [Fact]
    public void ShouldCallUpdateWithExpectedNewValues()
    {
        var coin = _fixture.Create<Coin>();
        SetupRepoMock(coin);
        Coin? callBack = null;
        _repository
            .Setup(x => x.Update(It.IsAny<Coin>()))
            .Callback<Coin>(x => callBack = x);

        var newCoin = _fixture.Create<Coin>();
        newCoin.Id = coin.Id;
        _sut.Update(newCoin);
        callBack.Should().BeEquivalentTo(newCoin);
    }

    [Fact]
    public void ShouldUpdateIfIdAndInGoldPassed()
    {
        var coin = _fixture.Create<Coin>();
        SetupRepoMock(coin);

        var newCoin = new Coin { Id = coin.Id, InGold = 10 };
        var result = _sut.Update(newCoin);
        result.Should().Be(Constants.Success);
    }

    // Delete
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