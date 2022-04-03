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
    public IActionResult Get(Guid id)
    {
        return Ok(_service.Get(id));
    }

    [HttpPut]
    public IActionResult Put([FromBody] Coin input)
    {
        if (string.IsNullOrEmpty(input.Name) || input.InGold == 0)
        {
            return BadRequest("Needed values not found");
        }
        var result = _service.Create(input);
        return result.Equals(Constants.Success, StringComparison.Ordinal) ? Ok(input) : BadRequest(result);
    }
    
    [HttpPost]
    public IActionResult Post([FromBody]Coin input)
    {
        if (input.Id == Guid.Empty && string.IsNullOrEmpty(input.Name))
        {
            return BadRequest("Must supply Name, unless changing name, then must supply id");
        }
        if (string.IsNullOrEmpty(input.Name) && input.InGold == 0)
        {
            return Delete(Guid.Empty);
        }
        var result = _service.Update(input);
        return result.Equals(Constants.Success, StringComparison.Ordinal) ? Ok(input) : BadRequest(result);
    }

    [HttpDelete]
    public IActionResult Delete(Guid id)
    {
        return _service.Delete(id) ? Ok("Coin Deleted") : NotFound($"Coin not found.");
    }
}