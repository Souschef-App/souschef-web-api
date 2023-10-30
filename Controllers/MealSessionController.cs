using Microsoft.AspNetCore.Mvc;
using souschef.server.Data.DTOs;
using souschef.server.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;

[Route("api/mealsessions")]
[ApiController]
public class MealSessionController : ControllerBase
{
    private readonly MealSessionRepository _repository;

    public MealSessionController(MealSessionRepository repository)
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
            DateTime = session.DateTime,
            Users = session.Users,
            Plan = session.Plan
        }).ToList();

        return Ok(sessionDtos);
    }

    [HttpGet("{id}")]
    public ActionResult<MealSessionDto> GetSession(Guid id)
    {
        var session = _repository.Get(id);
        if (session == null)
            return NotFound();

        var sessionDto = new MealSessionDto
        {
            Id = session.Id,
            DateTime = session.DateTime,
            // Map other properties here
            Users = session.Users,
            Plan = session.Plan
        };

        return Ok(sessionDto);
    }

    [HttpPost()]
    public ActionResult<MealSessionDto> CreateSession(MealSessionCreateDTO model)
    {
        var createdSession = _repository.Create(model);

        var sessionDto = new MealSessionDto
        {
            Id = createdSession.Id,
            DateTime = createdSession.DateTime,
            Users = createdSession.Users,
            Plan = createdSession.Plan
        };

        return CreatedAtAction(nameof(GetSession), new { id = sessionDto.Id }, sessionDto);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateSession(Guid id, MealSessionCreateDTO model)
    {
        var existingSession = _repository.Get(id);
        if (existingSession == null)
            return NotFound();

        existingSession.DateTime = model.DateTime;

        _repository.Update(existingSession);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteSession(Guid id)
    {
        var session = _repository.Get(id);
        if (session == null)
            return NotFound();

        _repository.Delete(id);
        return NoContent();
    }

    [HttpGet("{id}/users")]
    public ActionResult<ApplicationUser> GetSessionUsers(Guid id)
    {
        return Ok(_repository.Get(id).Users);
    }

    [HttpPost("{id}/users/{userId}")]
    public ActionResult<ApplicationUser> CreateSessionUsers(Guid id, Guid userId)
    {
        _repository.CreateSessionUser(id, userId);
        return Ok();
    }

    [HttpDelete("{id}/users/{userId}")]
    public ActionResult<ApplicationUser> DeleteSessionUsers(Guid id, Guid userId)
    {
        _repository.DeleteSessionUser(id, userId);
        return Ok();
    }
}
