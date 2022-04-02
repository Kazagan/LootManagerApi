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

    // [HttpPut]
    // public IActionResult Put([FromBody] CoinRoller coinRoller)
    // {
    //     var roller = _service.Create(coinRoller);
    //
    //     return roller.Id switch
    //     {
    //         -1 => BadRequest("Roller already exists"),
    //         -2 => BadRequest("Coin Not supplied or already exists"),
    //         _ => Ok(roller)
    //     };
    // }
}