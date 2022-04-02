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
    public IActionResult Get()
    {
        return Ok(_service.ReadAll());
    }

    [HttpGet]
    [Route("name/{name}")]
    public IActionResult Get(string name)
    {
        return Ok(_service.Read(name));
    }

    [HttpGet]
    [Route("{id}")]
    public IActionResult Get(Guid id)
    {
        return Ok(_service.Read(id));
    }

    [HttpPut]
    public IActionResult Put([FromBody] Coin input)
    {
        var coin = _service.Create(input);
        return Ok(coin);
    }
    
    [HttpPost]
    public IActionResult Post([FromBody]Coin input)
    {
        if (input.Id == Guid.Empty)
        {
            return BadRequest("Must provide id of valid coin");
        }
        var coin = _service.Update(input);
        return Ok(coin);
    }

    [HttpDelete]
    public IActionResult Delete(Guid id)
    {
        return _service.Delete(id) ? Ok("Coin Deleted") : NotFound($"Coin not found for {id}.");
    }
}