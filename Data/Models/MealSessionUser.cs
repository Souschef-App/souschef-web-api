using System.ComponentModel.DataAnnotations;

namespace souschef.server.Data.Models
{
    public class MealSessionUser
    {
        [Key]
        public Guid Id { get; set; }

        public MealSession Session { get; set; }
        public ApplicationUser User { get; set; }
    }
}
