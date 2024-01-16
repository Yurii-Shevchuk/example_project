using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models.HelperModels;

namespace api.Dtos.NutritionalDtos
{
    public class CreateNutritionalRequestDto
    {
        public int RecipeId { get; set; }
        public List<Ingredient> Ingredients { get; set; }
    }
}