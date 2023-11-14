using Microsoft.AspNetCore.Mvc;
using souschef.server.Data.DTOs;
using souschef.server.Data.Models;
using souschef.server.Data.Repository.Contracts;
using System.Diagnostics;

namespace souschef.server.Controllers;

[ApiController]
[Route("api/live-session")]
public class LiveSessionController : Controller
{
    private readonly ILiveSessionRepository m_liveSessionRepository;

    public LiveSessionController(ILiveSessionRepository _liveSessionRepository)
    {
        m_liveSessionRepository = _liveSessionRepository;
    }

    // TODO: Start live sessions by providing user and mealplan ID
    // 1. Validate that user has mealplan ID registered
    // 2. Create new LiveSession in database
    // 2.1 Generate session code & IP address
    // 3. Create instance of WebSocket server

    // TODO: Stop live sessions by code (called by WebSocket)
    // Note: Might need to add/use ID instead of session code

    [HttpGet("get-session")]
    public IActionResult GetIPBySessionCode([FromQuery] int code)
    {
        var session = m_liveSessionRepository.GetSessionByCode(code);

        if (session != null)
        {
            return Ok(session);
        }

        return new ContentResult
        {
            StatusCode = 404,
            Content = "Invalid session code"
        };
    }
}