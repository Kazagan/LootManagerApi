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
    public IActionResult Get(int treasureLevel, int roll)
    {
        if (roll != 0)
        {
            return Ok(_service.Get(treasureLevel, roll));
        }

        if (treasureLevel != 0)
        {
            return Ok(_service.Get(treasureLevel));
        }
        var rollers = _service.Get();
        return Ok(rollers);
    }

    [HttpGet]
    [Route("{id}")]
    public IActionResult Get(Guid id)
    {
        var roller = _service.Get(id);
        return roller is null ? NotFound(roller) : Ok(roller);
    }

    [HttpPost]
    public IActionResult Post([FromBody] CoinRoller coinRoller)
    {
        var result = _service.Create(coinRoller);
        return result.Equals(Constants.Success, StringComparison.Ordinal) ? Ok(coinRoller) : BadRequest(result);
    }

    [HttpPut]
    public IActionResult Put([FromBody] CoinRoller coinRoller)
    {
        if (coinRoller.Id == Guid.Empty)
        {
            return BadRequest("Must supply Id");
        }
        var result = _service.Update(coinRoller);
        return result switch
        {
            Constants.Success => Ok(coinRoller),
            Constants.NotFound => NotFound(),
            _ => BadRequest(result)
        };
    }

    [HttpDelete]
    public IActionResult Delete(Guid id)
    {
        return _service.Delete(id) ? Ok("Deleted") : NotFound();
    }
}