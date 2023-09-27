using souschef.server.Controllers;
using FakeItEasy;
using souschef.server.Data.Repository.Contracts;
using Microsoft.AspNetCore.Identity;
using souschef.server.Data.Models;
using Microsoft.AspNetCore.Mvc;
using souschef.server.Data.DTOs;

namespace souschef.Tests
{
    public class RecipeControllerTests
    {
        [Fact]
        public void Public_Add_Recipes_Adds_Recipe()
        {
            //Arrage
            var fakeRecipe = A.Dummy<Recipe>();
            var recipeDataStore = A.Fake<IRecipeRepository>();
            var userDataStore = A.Fake<UserManager<ApplicationUser>>();

            var fakeRecipeDTO = A.Dummy<RecipeDTO>();

            //A.CallTo(() => recipeDataStore.AddRecipe(fakeRecipe));

            //var controller = new RecipeController(recipeDataStore, userDataStore);

            ////Act
            //var actionRes = controller.AddRecipe(fakeRecipeDTO);

            ////Assert
            //var result = actionRes as OkObjectResult;
            //var returnRecipes = result?.Value as IEnumerable<Recipe>;


            //Assert.Equal(returnRecipes, fakeRecipes);
        }

        [Fact]
        public void Public_Get_Recipes_Returns_Recipes()
        {
            //Arrage
            var fakeRecipes = A.CollectionOfDummy<Recipe>(5);
            var recipeDataStore = A.Fake<IRecipeRepository>();
            var userDataStore = A.Fake<UserManager<ApplicationUser>>();
            A.CallTo(() => recipeDataStore.GetAll(null)).Returns(fakeRecipes);
            var controller = new RecipeController(recipeDataStore, userDataStore);

            //Act
            var actionRes = controller.GetAllRecipes();

            //Assert
            var result = actionRes.Result as OkObjectResult;
            var returnRecipes = result?.Value as IEnumerable<Recipe>;
            Assert.Equal(returnRecipes, fakeRecipes);
        }
    }
}