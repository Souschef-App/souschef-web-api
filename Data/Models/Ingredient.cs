using System;
using System.ComponentModel.DataAnnotations;

namespace souschef.server.Data.Models
{
    public class Ingredient
    {
        [Key]  
        public Guid     Id       { get; set; }
        public string?  Name     { get; set;}
        public Fraction? Quantity { get; set;}

        //None,
        //Ounces,     // Start of weight units
        //Pounds,
        //Grams,
        //Kilograms,  // End of weight units
        //Teaspoons,  // Start of volume units
        //Tablespoons,
        //Cups,
        //Pints,
        //Quarts,
        //Gallons,
        //Mililiters,
        //Liters,     // End of volume units
        public int      Unit     { get; set; }
    }
}
