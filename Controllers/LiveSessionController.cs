using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using souschef.server.Data.DTOs;
using souschef.server.Data.Models;
using souschef.server.Data.Repository.Contracts;
using souschef.server.Services.LiveSession;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Security.Principal;
using System.Text;
using Task = System.Threading.Tasks.Task;

namespace souschef.server.Controllers;

[ApiController]
[Route("api/live-session")]
public class LiveSessionController : Controller
{
    private readonly ILiveSessionRepository m_liveSessionRepository;
    private readonly ILiveSessionService m_liveSessionService;
    private readonly MealPlanRepository m_mealPlanRepository;

    public LiveSessionController(ILiveSessionRepository _liveSessionRepository, ILiveSessionService _liveSessionService, MealPlanRepository mealPlanRepository)
    {
        m_liveSessionRepository = _liveSessionRepository;
        m_liveSessionService = _liveSessionService;
        m_mealPlanRepository = mealPlanRepository;
    }

    async Task SendMessage(ClientWebSocket webSocket, ClientMessageDTO message)
    {
        string jsonMessage = JsonConvert.SerializeObject(message);
        byte[] buffer = Encoding.UTF8.GetBytes(jsonMessage);

        await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);

        Console.WriteLine($"Sent: {jsonMessage}");
    }

    [HttpPost("start-session")]
    public async Task<IActionResult> StartSession()
    {
        var ipAddress = await m_liveSessionService.Start();

        if (ipAddress != null)
        {
            string webSocketURL = $"ws://{ipAddress}/ws";

            // Default mealplan
            var mealplans = m_mealPlanRepository.GetAll().ToList();
            if (mealplans.Count == 0)
            {
                throw new Exception("Default Mealplan doesn't exist!");
            }

            MealPlan defaultMealplan = mealplans[0];
            MealPlanDTO mealPlan = new MealPlanDTO
            {
                Id = defaultMealplan.Id.ToString(),
                Name = defaultMealplan.Name,
                Date = defaultMealplan.Date.Ticks,
                HostId = defaultMealplan?.ApplicationUser?.Id?.ToString() ?? "",
                Recipes = defaultMealplan?.Recipes.ToArray() ?? new Recipe[0],
                OccasionType = 0,
            };

            using (ClientWebSocket webSocket = new())
            {
                try
                {
                    Console.WriteLine($"Connecting to {webSocketURL}...");

                    await webSocket.ConnectAsync(new Uri(webSocketURL), CancellationToken.None);

                    Console.WriteLine("Connected!");

                    var message = new ClientMessageDTO
                    {
                        Type = "session_create",
                        Payload = mealPlan
                    };

                    await SendMessage(webSocket, message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex}");
                }
                finally
                {
                    if (webSocket.State == WebSocketState.Open)
                    {
                        await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                    }
                }
            }


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
    public IActionResult StopSession([FromQuery] int code)
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