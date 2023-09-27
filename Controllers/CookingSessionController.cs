using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using souschef.server.Data.DTOs;
using souschef.server.Data.Models;
using souschef.server.Data.Repository.Contracts;
using souschef.server.Helpers;

namespace souschef.server.Controllers
{
    [ApiController]
    [Route("api/cookingsession")]
    public class CookingSessionController : Controller
    {
        private readonly ICookingSessionRepository     m_cookingSessionRepository;
        private readonly UserManager<ApplicationUser>  m_userManager;
        private readonly IRecipeRepository             m_recipeRepository;

        public CookingSessionController(ICookingSessionRepository _cookingSessionRepository, UserManager<ApplicationUser> _userManager, IRecipeRepository _recipeRepository)
        {
            m_cookingSessionRepository = _cookingSessionRepository;
            m_userManager = _userManager;
            m_recipeRepository = _recipeRepository;
        }
        
        [HttpGet("get-todays-cooking-session-by-user")]
        public async Task<ActionResult> GetTodaysCookingSessionByUser([FromQuery] string userId)
        {
            var user = await m_userManager.FindByIdAsync(userId);

            if(user == null)
            {
                return new ContentResult() { Content = "User Not Found", StatusCode = 404 };
            }

            var sessions =  m_cookingSessionRepository.GetCookingSessionsByUser(Guid.Parse(userId));

            List<CookingSession> todaysSessions = new();

            Console.WriteLine("Session Number " + sessions?.Count());

            foreach (var session in sessions!)
            {
                if (session.Date >= Conversions.GetUnixTimeStamp(DateTime.Now.Date))
                {
                    todaysSessions.Add(session);
                }
            }

            if(todaysSessions?.Count() <= 0)
                return new ContentResult() { Content = "User doesn't have cooking session today", StatusCode = 404 };

            Console.WriteLine(todaysSessions);

            return Ok(todaysSessions);
        }

        [HttpPost("save-cooking-session")]
        public async Task<ActionResult> SaveCookingSession([FromBody] CookingSessionDTO cookingSessionDTO)
        {

            if(cookingSessionDTO.RecipeIds == null)
                return new ContentResult() { Content = "Cooking Session Must Have Recipe Ids", StatusCode = 404 };

            List<Recipe> referecedRecipes = new();

            foreach (var id in cookingSessionDTO.RecipeIds)
            {
                var r = m_recipeRepository.GetRecipe(Guid.Parse(id));

                if(r !=  null)
                {
                    referecedRecipes.Add(r);
                }
            }

            var host = await m_userManager.FindByIdAsync(cookingSessionDTO.HostId);

            CookingSession cookingSession = new CookingSession
            {
                Id = Guid.NewGuid(),
                Name = cookingSessionDTO.Name,
                Recipes = referecedRecipes,
                Date = Conversions.GetUnixTimeStamp(DateTime.Now),
                Host = host,
                OccasionType = cookingSessionDTO.OccasionType,
            };

            m_cookingSessionRepository.SaveCookingSession(cookingSession);

            return Ok();
        }
    }
}
