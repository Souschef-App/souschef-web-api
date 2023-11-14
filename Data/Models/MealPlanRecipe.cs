using System.ComponentModel.DataAnnotations;

namespace souschef.server.Data.Models
{
    public class MealPlanRecipe
    {
        [Key]
        public Guid Id { get; set; }

        public MealPlan MealPlan { get; set; }
        public Recipe Recipe { get; set; }

        public string MealType { get; set; }
        public int Order { get; set; }
    }
}
