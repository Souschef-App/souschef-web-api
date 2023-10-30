using souschef.server.Data.Models;

namespace souschef.server.Data.DTOs
{
    public class MealSessionCreateDTO
    {
        public Guid Id { get; set; }
        public DateTime DateTime { get; set; }

        public Guid PlanId { get; set; }
    }
}
