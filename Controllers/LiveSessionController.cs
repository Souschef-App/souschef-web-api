using Microsoft.AspNetCore.Mvc;
using souschef.server.Data.DTOs;
using souschef.server.Data.Models;
using souschef.server.Data.Repository.Contracts;
using souschef.server.Services.LiveSession;
using System.Diagnostics;

namespace souschef.server.Controllers;

[ApiController]
[Route("api/live-session")]
public class LiveSessionController : Controller
{
    private readonly ILiveSessionRepository m_liveSessionRepository;
    private readonly ILiveSessionService m_liveSessionService;

    public LiveSessionController(ILiveSessionRepository _liveSessionRepository, ILiveSessionService _liveSessionService)
    {
        m_liveSessionRepository = _liveSessionRepository;
        m_liveSessionService = _liveSessionService;
    }

    // TODO: Start live sessions by providing user and mealplan ID
    // 1. Validate that user has mealplan ID registered
    // 2. Create instance of WebSocker server
    // 3. Add new LiveSession in database

    [HttpPost("start-session")]
    public async Task<IActionResult> StartSession()
    {
        var ipAddress = await m_liveSessionService.Start();

        if (ipAddress != null)
        {
            var session = m_liveSessionRepository.CreateSession(ipAddress);

            if (session != null)
            {
                return Ok(session);
            }
            else
            {
                // TODO: Stop and remove container b/c database failed
            }
        }

        return new ContentResult
        {
            StatusCode = 400,
            Content = "Failed to start live session"
        };
    }

    // TODO: Stop live sessions by code (called by WebSocket)
    // - Update DB to store docker container ID inside LiveSession
    // - Make LiveSessionDTO that doesn't include dockerID

    [HttpPost("stop-session")]
    public async Task<IActionResult> StopSession([FromQuery] int code)
    {
        var success = m_liveSessionRepository.DeleteSessionByCode(code);

        if (success)
        {
            return Ok();
        }

        return new ContentResult
        {
            StatusCode = 400,
            Content = "Failed to stop live session"
        };
    }

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