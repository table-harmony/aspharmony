using BusinessLogicLayer.Services;
using Microsoft.AspNetCore.Mvc;
using DataAccessLayer.Entities;

namespace WebApi.Controllers;

[Route("api/feedback")]
[ApiController]
public class FeedbackController(IFeedbackService feedbackService) : ControllerBase {
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Feedback>>> GetAll() {
        var feedback = await feedbackService.GetAllAsync();
        return Ok(feedback);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Feedback>> Get(int id) {
        var feedback = await feedbackService.GetAsync(id);
        if (feedback == null) return NotFound();
        return Ok(feedback);
    }

    [HttpPost]
    public async Task<ActionResult<Feedback>> Create([FromBody] Feedback feedback) {
        await feedbackService.CreateAsync(feedback);
        return CreatedAtAction(nameof(Get), new { id = feedback.Id }, feedback);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Feedback feedback) {
        if (id != feedback.Id) return BadRequest();
        await feedbackService.UpdateAsync(feedback);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id) {
        await feedbackService.DeleteAsync(id);
        return NoContent();
    }
} 