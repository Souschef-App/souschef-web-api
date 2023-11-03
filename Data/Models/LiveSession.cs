using System;
using System.ComponentModel.DataAnnotations;

namespace souschef.server.Data.Models
{
    public class LiveSession
    {
        [Key]
        public int? Code { get; set; }

        public string? IP { get; set; }
    }
}