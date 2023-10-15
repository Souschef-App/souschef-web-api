using Microsoft.AspNetCore.Mvc;
using souschef.server.Data.Repository.Contracts;

namespace souschef.server.Controllers;

[ApiController]
[Route("api/live-cookingsession")]
public class LiveCookingSessionController : Controller
{
    private readonly ICookingSessionRepository m_cookingSessionRepository;

    public LiveCookingSessionController(ICookingSessionRepository _cookingSessionRepository)
    {
        m_cookingSessionRepository = _cookingSessionRepository;
    }

    [HttpGet("get-ip")]
    public IActionResult GetIPBySessionCode([FromQuery] int code)
    {
        var sessionIP = m_cookingSessionRepository.GetIP(code);

        if (sessionIP != null)
        {
            return Ok(sessionIP!);
        }

        return new ContentResult() { Content = "Invalid session code", StatusCode = 404 };
    }
}