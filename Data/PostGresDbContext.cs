using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using souschef.server.Data.Models;

namespace souschef.server.Data;

public class PostGresDBContext : IdentityDbContext<ApplicationUser>
{
    public PostGresDBContext(DbContextOptions<PostGresDBContext> options) : base(options)
    {

    }

    public DbSet<ApplicationUser>? ApplicationUsers { get; set; }
    public DbSet<CookingSession>? CookingSessions { get; set; }
    public DbSet<Recipe>? Recipes { get; set; }
    public DbSet<Models.Task>? Tasks { get; set; }
    public DbSet<Ingredient>? Ingredients { get; set; }
    public DbSet<Kitchenware>? Kitchenware { get; set; }

    public DbSet<MealPlanRecipe>? MealPlanRecipes { get; set; }
    public DbSet<MealSessionUser>? MealSessionUsers { get; set; }

    public DbSet<MealPlan>? MealPlans { get; set;}
    public DbSet<MealSession>? MealSessions { get; set;}
    public DbSet<FavoriteRecipe> FavoriteRecipes { get; set; }
}