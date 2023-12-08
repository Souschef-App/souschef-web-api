using Microsoft.EntityFrameworkCore;
using souschef.server.Data.Models;
using souschef.server.Data.Repository.Contracts;

namespace souschef.server.Data.Repository
{
    public class RecipeRepository : IRecipeRepository
    {
        private readonly PostGresDBContext _context;

        public RecipeRepository(PostGresDBContext context) { _context = context; }

        public IEnumerable<Recipe>? Recipes => _context.Recipes?
               .Include(c => c.Ingredients)
               .Include(c => c.Kitchenware)
               .Include(c => c.Tasks).ThenInclude(x => x.Ingredients)
               .Include(c => c.Tasks).ThenInclude(x => x.Kitchenware)
               .Include(c => c.Tasks).ThenInclude(x => x.Dependencies);

        public void AddRecipe(Recipe _recipe)
        {
            _context.Recipes?.AddAsync(_recipe);
            _context.SaveChanges();
        }

        public void DeleteRecipe(Recipe _recipe)
        {
            _context.Recipes?.Remove(_recipe);
            _context.SaveChanges();
        }

        public IEnumerable<Recipe>? GetAll(Guid? ownerId)
        {
            return Recipes?.Where(r => r.OwnerId == ownerId);
        }

        public Recipe? GetRecipe(Guid recipeId)
        {
            return Recipes?.Where(r => r.Id == recipeId).First();
        }

        public bool Modify(Recipe _recipe)
        {
            throw new NotImplementedException();
        }
    }
}
