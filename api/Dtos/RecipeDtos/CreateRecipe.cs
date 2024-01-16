using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using api.Models.HelperModels;

namespace api.Dtos.RecipeDtos
{
    public class CreateRecipe
    {   
        [Required]
        [MaxLength(50, ErrorMessage = "Title cannot be over 50 characters")]
        public string Title { get; set; }
        [Required]
        [MaxLength(1024, ErrorMessage = "Recipe text cannot be over 1024 characters")]
        public string Text { get; set; }
        [Required]
        public List<Ingredient> RecipeIngredients { get; set; }

    }
}