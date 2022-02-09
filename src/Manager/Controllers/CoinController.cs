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
    [Route("{id}")]
    public IActionResult Get(int id)
    {
        return Ok(_service.Get(id));
    }

    [HttpPut]
    public IActionResult Put([FromBody] Coin input)
    {
        var coin = _service.Create(input);
        return coin.Id switch
        {
            -1 => BadRequest("Invalid format passed, must pass Name and inGold"),
            0 => Conflict($"Coin Type {input.Name} already exists"),
            _ => Ok(coin)
        };
    }
    [HttpPost]
    public IActionResult Post([FromBody]Coin input)
    {
        if (input.Id == 0)
        {
            return BadRequest("Must provide id of valid coin");
        }
        var coin = _service.Update(input);
        return coin.Id switch
        {
            -1 => NotFound("Coin not found."),
            0 => Conflict($"Coin Type {input.Name} already exists"),
            _ => Accepted(coin)
        };
    }

    [HttpDelete]
    public IActionResult Delete(int id)
    {
        return _service.Delete(id) ? Ok("Coin Deleted") : NotFound($"Coin not found for {id}.");
    }
}