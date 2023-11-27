using System.ComponentModel.DataAnnotations;

namespace souschef.server.Data.Models
{
    public class Fraction
    {
        [Key]
        public Guid Id { get; set; }
        public int Whole { get; set; }
        public int Numerator { get; set; }
        public int Denominator { get; set; }
    }
}
