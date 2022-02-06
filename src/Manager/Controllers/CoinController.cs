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
    public IActionResult Get()
    {
        return Ok(_service.GetAll());
    }

    [HttpGet]
    public IActionResult Get(string name)
    {
        return Ok(_service.Get(name));
    }

    [HttpGet]
    public IActionResult Get(int id)
    {
        return Ok(_service.Get(id));
    }

    [HttpPut]
    public IActionResult Put(string name, double inGold)
    {
        var x = _service.Create(name, inGold);
        if (x is null)
        {
            return BadRequest("Coin Type already exists");
        }
        return Ok();
    }

    [HttpPost]
    public IActionResult Post(int id, string? name, double? inGold)
    {
        var x = _service.Update(id, name, inGold);
        return x.Id switch
        {
            0 => BadRequest("Coin Not Found"),
            -1 => BadRequest("Coin Type already exists"),
            _ => Ok(x)
        };
    }
}