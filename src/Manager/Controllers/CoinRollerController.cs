using Data.Entities;
using Data.Repositories;
using Manager.Services;
using Microsoft.AspNetCore.Mvc;

namespace Manager.Controllers;

[Route("[Controller]")]
public class CoinRollerController : ControllerBase
{
    private readonly CoinRollerService _service;

    public CoinRollerController(IRepository repository)
    {
        _service = new CoinRollerService(repository);
    }

    [HttpGet]
    [Route("{id}")]
    public IActionResult Get(Guid id)
    {
        var roller = _service.Get(id);
        return roller is null ? NotFound(roller) : Ok(roller);
    }

    [HttpGet]
    public IActionResult Get(int treasureLevel, int roll)
    {
        if (roll != 0)
        {
            var result = _service.Get(treasureLevel, roll);
            return result is null ? NotFound() : Ok(result);
        }
        else if (treasureLevel != 0)
        {
            var results = _service.Get(treasureLevel);
            return !results.Any() ? NotFound() : Ok(results);
        }
        else
        {
            var results = _service.Get();
            return !results.Any() ? NotFound() : Ok(results);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CoinRoller coinRoller)
    {
        var result = await _service.Create(coinRoller);
        return result switch
        {
            Constants.Exists => BadRequest(Constants.Exists),
            Constants.Invalid => BadRequest(Constants.Invalid),
            _ => new ContentResult { Content = result, StatusCode = 201 }
        };
    }

    [HttpPut]
    public async Task<IActionResult> Put([FromBody] CoinRoller coinRoller)
    {
        if (coinRoller.Id == Guid.Empty)
        {
            return await Post(coinRoller);
        }
        var result = await _service.Update(coinRoller);
        return result switch
        {
            Constants.NotFound => NotFound(),
            Constants.Invalid => BadRequest(Constants.Invalid),
            Constants.Success => Ok("Updated"),
            _ => BadRequest()
        };
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(Guid id)
    {
        return await _service.Delete(id) ? Ok("Deleted") : NotFound();
    }
}