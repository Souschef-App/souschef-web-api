using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

[Route("api/sessions")]
[ApiController]
public class SessionController : ControllerBase
{
    private readonly SessionRepository _repository;

    public SessionController(SessionRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public ActionResult<IEnumerable<MealSessionDto>> GetSessions()
    {
        var sessions = _repository.GetAll();
        var sessionDtos = sessions.Select(session => new MealSessionDto
        {
            Id = session.Id,
            DateTime = session.DateTime
            // Map other properties here
        }).ToList();

        return Ok(sessionDtos);
    }

    [HttpGet("{id}")]
    public ActionResult<MealSessionDto> GetSession(int id)
    {
        var session = _repository.Get(id);
        if (session == null)
            return NotFound();

        var sessionDto = new MealSessionDto
        {
            Id = session.Id,
            DateTime = session.DateTime
            // Map other properties here
        };

        return Ok(sessionDto);
    }

    [HttpPost]
    public ActionResult<MealSessionDto> CreateSession(MealSession model)
    {
        // You can perform validation here if needed
        var createdSession = _repository.Create(model);

        var sessionDto = new MealSessionDto
        {
            Id = createdSession.Id,
            DateTime = createdSession.DateTime
            // Map other properties here
        };

        return CreatedAtAction(nameof(GetSession), new { id = sessionDto.Id }, sessionDto);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateSession(int id, MealSession model)
    {
        var existingSession = _repository.Get(id);
        if (existingSession == null)
            return NotFound();

        existingSession.DateTime = model.DateTime;
        // Update other properties as needed

        _repository.Update(existingSession);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteSession(int id)
    {
        var session = _repository.Get(id);
        if (session == null)
            return NotFound();

        _repository.Delete(id);
        return NoContent();
    }
}
