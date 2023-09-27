using souschef.server.Data.Models;

namespace souschef.server.Data.Repository.Contracts
{
    public interface IRecipeRepository
    {
        void AddRecipe(Recipe _recipe);
        void DeleteRecipe(Recipe _recipe);
        Recipe? GetRecipe(Guid recipeId);
        IEnumerable<Recipe>? GetAll(Guid? ownerId);
        bool Modify(Recipe _recipe);
    }
}
