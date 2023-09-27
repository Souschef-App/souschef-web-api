using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using souschef.server.Data.DTOs;
using souschef.server.Data.LiveSession;
using souschef.server.Data.Models;
using souschef.server.Data.Repository.Contracts;
using souschef.server.Helpers;

namespace souschef.server.Controllers;

[ApiController]
[Route("api/live-cookingsession")]
public class LiveCookingSessionController : Controller
{
    private readonly ICookingSessionRepository m_cookingSessionRepository;
    private readonly UserManager<ApplicationUser> m_userManager;

    public LiveCookingSessionController(ICookingSessionRepository _cookingSessionRepository, UserManager<ApplicationUser> _userManager)
    {
        m_cookingSessionRepository = _cookingSessionRepository;
        m_userManager = _userManager;
    }

    [HttpGet("start")]
    public ActionResult Start(string sessionId)
    { 
        var session = m_cookingSessionRepository.GetCookingSessionsById(Guid.Parse(sessionId));

        Console.WriteLine("Recipe Count " + session!.Recipes?.Count);

        if(session != null)
        {
            var liveSession = LiveSessions.GetLiveSessions().StartCookingSession(session);

            if (liveSession != null)
            {
                return Ok(liveSession.Id);
            }
            else
            {
                return new ContentResult() { Content = "Start session failed", StatusCode = 404 };
            }
        }
        else
        {
            return new ContentResult() { Content = "Couldn't find session", StatusCode = 404 };
        }
    }

    [HttpGet("end")]
    public IActionResult End([FromQuery] string sessionId)
    {
        if (LiveSessions.GetLiveSessions().RemoveSessionById(Guid.Parse(sessionId)))
        {
                return Ok();
            }

        return new ContentResult() { Content = "Invalid session ID", StatusCode = 404 };
    }

    [HttpPost("join")]
    public async Task<IActionResult> Join([FromQuery] string sessionId, [FromQuery] string userId)
    {
        var user = await m_userManager.FindByIdAsync(userId);
        var session = LiveSessions.GetLiveSessions().GetSessionById(Guid.Parse(sessionId));

        if (session != null)
        {
            session.Members.Add(Conversions.ToUserDTO(user));
            return Ok();
        }

        return new ContentResult() { Content = "Invalid session ID", StatusCode = 404 };
    }

    [HttpPost("leave")]
    public async Task<IActionResult> Leave([FromQuery] string sessionId)
    {
        // var user = await m_userManager.FindByIdAsync(userId);
        var session = LiveSessions.GetLiveSessions().GetSessionById(Guid.Parse(sessionId));

        if (session != null)
        {
            // session.Members.Remove(user);
            return Ok();
        }

        return new ContentResult() { Content = "Invalid session ID", StatusCode = 404 };
    }

    [HttpGet("get-users")]
    public IEnumerable<ApplicationUser> GetUsers([FromQuery] string sessionId)
    {
        return m_cookingSessionRepository.GetUsers(Guid.Parse(sessionId));
    }

    [HttpGet("get-task")]
    public ActionResult<Data.Models.Task> GetTask([FromQuery] string sessionId, [FromQuery] string userId)
    {
        var session = LiveSessions.GetLiveSessions().GetSessionById(Guid.Parse(sessionId));
        if (session != null)
        {
            var task = session.GetNextTask(userId);
            if (task != null) return Ok(task);
            return new ContentResult() { Content = "No more tasks available", StatusCode = 404 };
        }

        return new ContentResult() { Content = "Invalid session ID", StatusCode = 404 };
    }

    [HttpGet("re-roll-task")]
    public ActionResult<Data.Models.Task> ReRollTask([FromQuery] string sessionId, [FromQuery] string userId, [FromQuery] string taskId)
    {
        var session = LiveSessions.GetLiveSessions().GetSessionById(Guid.Parse(sessionId));
        if (session != null)
        {
            var user = session.GetUser(userId);

            if(user.CurrentRecipe == null)
                new ContentResult() { Content = "User Not Working On Anything", StatusCode = 404 };


            // 1. Call Next Task while the task you had is inprogress!
            var task = session.GetNextTask(userId);

            // 2. Switch Old Task to not in progress to free it up 
            session.Recipes[(Guid)user.CurrentRecipe!].GetTask(Guid.Parse(taskId)).InProgress = false;

            if (task != null) 
                return Ok(task);
            return 
                new ContentResult() { Content = "No more tasks available", StatusCode = 404 };
        }

        return new ContentResult() { Content = "Invalid session ID", StatusCode = 404 };
    }

    [HttpPost("complete-task")]
    public IActionResult CompleteTask([FromQuery] string sessionId, [FromQuery] string userId, [FromQuery] string taskId)
    {
        var session = LiveSessions.GetLiveSessions().GetSessionById(Guid.Parse(sessionId));

        if (session != null)
        {
            var u = session.GetUser(userId);
            if(u.CurrentRecipe != null)
            {
                Guid id = (Guid)u.CurrentRecipe;
                session.Recipes[id].GetTask(Guid.Parse(taskId)).Finished = true;
                Console.WriteLine("rec " + session.Recipes[id].GetTask(Guid.Parse(taskId)).Finished);
                return Ok();
            }
            else
            {
                return new ContentResult() { Content = "User is not currently working on a recipe", StatusCode = 404 };
            }
        }

        return new ContentResult() { Content = "Invalid session ID", StatusCode = 404 };
    }
}