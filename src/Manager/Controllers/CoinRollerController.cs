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
    public IActionResult Get()
    {
        var rollers = _service.GetAll();
        return Ok(rollers);
    }

    [HttpGet]
    [Route("{id}")]
    public IActionResult Get(Guid id)
    {
        var roller = _service.Get(id);
        return Ok(roller);
    }

    [HttpGet]
    [Route("level")]
    public IActionResult Get(int treasureLevel, int roll)
    {
        if (roll == 0)
        {
            var coinRollers = _service.GetAll().Where(x => x.TreasureLevel == treasureLevel);
            return Ok(coinRollers);
        }
        var coinRoller = _service.Get(treasureLevel, roll);
        return Ok(coinRoller);
    }

    [HttpPut]
    public IActionResult Put([FromBody] CoinRoller coinRoller)
    {
        var result = _service.Create(coinRoller);
        return result.Equals(Constants.Success, StringComparison.Ordinal) ? Ok(coinRoller) : BadRequest(result);
    }

    [HttpPost]
    public IActionResult Post([FromBody] CoinRoller coinRoller)
    {
        return Ok();
    }
}