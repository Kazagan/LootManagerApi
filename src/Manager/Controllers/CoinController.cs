using Data.Entities;
using Data.Repositories;
using Manager.Services;
using Microsoft.AspNetCore.Mvc;

namespace Manager.Controllers;

[Route("[Controller]")]
public class CoinController : ControllerBase
{
    private readonly CoinService _service;

    public CoinController(IRepository repository)
    {
        _service = new CoinService(repository);
    }

    [HttpGet]
    public IActionResult Get(string? name)
    {
        if (name is null)
        {
            return Ok(_service.Get());
        }
        var result = _service.Get(name);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet]
    [Route("{id}")]
    public IActionResult Get(Guid id)
    {
        var result = _service.Get(id);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public IActionResult Create([FromBody] Coin input)
    {
        var result = _service.Create(input);
        return result switch
        {
            Constants.Invalid => BadRequest("Invalid Coin."),
            Constants.Exists => BadRequest("Coin already exists."),
            _ => new ContentResult() { Content = result, StatusCode = 201 }
        };
    }

    [HttpPut]
    public IActionResult Update([FromBody] Coin input)
    {
        var result = _service.Update(input);
        return result switch
        {
            Constants.Invalid => BadRequest("Coin doesn't exist, creation failed. Invalid"),
            Constants.Exists => BadRequest("Name taken"),
            Constants.Success => Ok("Updated"),
            _ => new ContentResult() { Content = result, StatusCode = 201 }
        };
    }

    [HttpDelete]
    public IActionResult Delete(Guid id)
    {
        return _service.Delete(id) ? Ok("Deleted") : NotFound();
    }
}