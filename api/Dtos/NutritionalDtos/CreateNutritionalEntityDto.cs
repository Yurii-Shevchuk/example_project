using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.NutritionalDtos
{
    public class CreateNutritionalEntityDto
    {
        public int RecipeId { get; set; }
        public string Data { get; set; } 
    }
}