using System;
using System.ComponentModel.DataAnnotations;

namespace souschef.server.Data.Models
{
    public class CookingSession
    {
        [Key]
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public long? Date { get; set; }

        public int? OccasionType { get; set; }

        public List<Recipe>? Recipes { get; set; }

        public List<ApplicationUser> Guests { get; set; } = new();

        public ApplicationUser? Host { get; set; }

        public Guid OwnerId { get; set; }
    }
}