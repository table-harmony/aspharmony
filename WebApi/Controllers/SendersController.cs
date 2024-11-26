using BusinessLogicLayer.Services;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/senders")]
[ApiController]
public class SendersController(ISenderService senderService) : ControllerBase {
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Sender>>> GetAll() {
        var senders = await senderService.GetAllAsync();
        return Ok(senders);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Sender>> Get(int id) {
        var sender = await senderService.GetAsync(id);
        if (sender == null) return NotFound();
        return Ok(sender);
    }
} 