using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using souschef.server.Data.DTOs;
using souschef.server.Data.Models;
using souschef.server.Data.Repository.Contracts;
using souschef.server.Helpers;

namespace souschef.server.Controllers
{
    [ApiController]
    [Route("api/recipe")]
    public class RecipeController : Controller
    {
        private readonly IRecipeRepository m_recipeRepository;
        private readonly UserManager<ApplicationUser> m_userManager;

        public RecipeController(IRecipeRepository _recipeRepository, UserManager<ApplicationUser> _userManager)
        {
            m_recipeRepository = _recipeRepository;
            m_userManager = _userManager;
        }

        [HttpPost("add-recipe")]
        public IActionResult AddRecipe([FromBody] RecipeDTO _dto)
        {
            if (_dto.OwnerId != null && _dto.Tasks != null)
            {
                var recipe = new Recipe()
                {
                    Id = Guid.NewGuid(),
                    Name = _dto.Name,
                    Duration = (float)_dto.Duration!,
                    Serves = _dto.Serves,
                    Date = Conversions.GetUnixTimeStamp(DateTime.Now),
                    Tasks = Array.ConvertAll(_dto.Tasks, new Converter<TaskDTO, Data.Models.Task>(delegate (TaskDTO x) { return Conversions.ToTask(x)!; })).ToList(), //Fix Null Issue
                    OwnerId = Guid.Parse(_dto.OwnerId),
                    Difficulty = _dto.Difficulty,
                    Kitchenware = _dto.KitchenWare?.ToList(),
                    Ingredients = _dto.Ingredients?.ToList(),
                };

                m_recipeRepository.AddRecipe(recipe);

                Console.WriteLine("Saved Recipe");

                return Ok("Success");
            }
            else
            {
                return new ContentResult() { Content = "Invalid Owner Id", StatusCode = 404 };
            }
        }

        [HttpPost("public-recipe")]
        public IActionResult AddPublicRecipe([FromBody] RecipeDTO _dto)
        {
            if (_dto.Tasks != null)
            {
                var recipe = new Recipe()
                {
                    Id = Guid.NewGuid(),
                    Name = _dto.Name,
                    Duration = (float)_dto.Duration!,
                    Serves = _dto.Serves,
                    Date = Conversions.GetUnixTimeStamp(DateTime.Now),
                    Tasks = Array.ConvertAll(_dto.Tasks, new Converter<TaskDTO, Data.Models.Task>(delegate (TaskDTO x) { return Conversions.ToTask(x)!; })).ToList(), //Fix Null Issue
                    OwnerId = null,
                    Difficulty = _dto.Difficulty,
                    Kitchenware = _dto.KitchenWare?.ToList(),
                    Ingredients = _dto.Ingredients?.ToList(),
                };

                m_recipeRepository.AddRecipe(recipe);
                return Ok();
            }
            else
            {
                return new ContentResult() { Content = "Invalid Recipe", StatusCode = 404 };
            }
        }

        [HttpGet("public-recipes")]
        public ActionResult<IEnumerable<Recipe>> GetAllRecipes()
        {
            var recipes = m_recipeRepository.GetAll(null);
            if (recipes != null)
            {
                return Ok(recipes);
            }
            else
            {
                return new ContentResult() { Content = "Recipes Not Found", StatusCode = 404 };
            }
        }

        [HttpGet("get-my-recipes")]
        public ActionResult<IEnumerable<Recipe>> GetMyRecipes([FromQuery]string _ownerId)
        {
            if (!Guid.TryParse(_ownerId, out Guid res))
            {
                return new ContentResult() { Content = "Couldn't parse GUID", StatusCode = 500 };
            }

            var recipes = m_recipeRepository.GetAll(res);

            if (recipes == null)
            {
                return new ContentResult() { Content = "Recipes Not Found", StatusCode = 404 };
            }

            return Ok(recipes);
        }

        [HttpPatch()]
        public ActionResult ModifyRecipe([FromBody] RecipeDTO _dto)
        {
            if (_dto != null && _dto.OwnerId != null && _dto.Tasks != null)
            {
                var recipe = new Recipe()
                {
                    Id = Guid.NewGuid(),
                    Duration = (float)_dto.Duration!,
                };

                m_recipeRepository.Modify(recipe);
                return Ok();
            }
            else
            {
                return new ContentResult() { Content = "Delete Failed", StatusCode = 404 };
            }
        }

        [HttpDelete()]
        public ActionResult DeleteRecipe(string _recipeId)
        {
            Recipe? recipe = m_recipeRepository.GetRecipe(Guid.Parse(_recipeId));

            if (recipe != null)
            {
                m_recipeRepository.DeleteRecipe(recipe);
                return Ok();
            }
            else
            {
                return new ContentResult() { Content = "Delete Failed", StatusCode = 404 };
            }
        }
    }
}
