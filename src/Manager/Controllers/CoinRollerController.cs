using Data.Entities;
using Data.Repositories;
using Manager.Services;
using Microsoft.AspNetCore.Mvc;

namespace Manager.Controllers;

[Route("[Controller")]
public class CoinRollerController : ControllerBase
{
    private readonly CoinRollerService _service;

    public CoinRollerController(IRepository<ManagerContext> repository)
    {
        _service = new CoinRollerService(repository);
    }

    [HttpPut]
    public IActionResult Put([FromBody] CoinRoller coinRoller)
    {
        var roller = _service.Create(coinRoller);
        return Ok();
    }
}