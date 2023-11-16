using System;
using System.ComponentModel.DataAnnotations;

namespace souschef.server.Data.Models
{
    public class Kitchenware
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }
        public int Quantity { get; set; }
    }
}