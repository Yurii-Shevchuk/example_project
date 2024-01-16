using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class NutritionalData
    {
        public int NutritionalDataId { get; set; }
        public int RecipeId { get; set; }
        public string Data { get; set; } //should be JSON
        public Recipe Recipe { get; set; }
    }
}