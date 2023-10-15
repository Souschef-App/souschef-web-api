using System;
using System.ComponentModel.DataAnnotations;

namespace souschef.server.Data.Models
{
    public class CookingSession
    {
        [Key]
        public int? Code { get; set; }

        public string? IP { get; set; }
    }
}