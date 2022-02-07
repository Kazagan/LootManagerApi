using Data.Entities;
using Data.Repositories;
using Manager.Services;
using Microsoft.AspNetCore.Mvc;

namespace Manager.Controllers;

[Route("[Controller]")]
public class CoinController : ControllerBase
{
    private readonly CoinService _service;

    public CoinController(IRepository<ManagerContext> repository)
    {
        _service = new CoinService(repository);
    }

    [HttpGet]
    [Route("All")]
    public IActionResult Get()
    {
        return Ok(_service.GetAll());
    }

    [HttpGet]
    [Route("name/{name}")]
    public IActionResult Get(string name)
    {
        return Ok(_service.Get(name));
    }

    [HttpGet]
    [Route("id/{id}")]
    public IActionResult Get(int id)
    {
        return Ok(_service.Get(id));
    }

    [HttpPut]
    public IActionResult Put(string name, decimal inGold)
    {
        var coin = _service.Create(name, inGold);
        if (coin is null)
        {
            return Conflict("Coin Type already exists");
        }
        return Created(coin.Id.ToString(), coin);
    }
    [HttpPost]
    public IActionResult Post(int id, string? name, decimal inGold)
    {
        var coin = _service.Update(id, name, inGold);
        return coin.Id switch
        {
            -1 => NotFound("Coin not found."),
            0 => Conflict("Coin with that name already exists."),
            _ => Accepted(coin)
        };
    }

    [HttpDelete]
    public IActionResult Delete(int id)
    {
        return _service.Delete((int) id) ? Ok("Coin Deleted") : NotFound($"Coin not found for {id}.");
    }
}