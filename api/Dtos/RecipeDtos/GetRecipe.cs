using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.CommentDtos;
using api.Models;
using api.Models.HelperModels;

namespace api.Dtos.RecipeDtos
{
    public class GetRecipe
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public List<Ingredient> RecipeIngredients { get; set; }
        public List<GetComment> Comments { get; set; }
    }
}