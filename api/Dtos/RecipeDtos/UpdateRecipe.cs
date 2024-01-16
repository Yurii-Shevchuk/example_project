using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Dtos.RecipeDtos
{
    public class UpdateRecipe
    {
        [Required]
        [MaxLength(50, ErrorMessage = "Title cannot be over 50 characters")]
        public string Title { get; set; }
        [Required]
        [MaxLength(1024, ErrorMessage = "Recipe text can't be over 1024 characters")]
        public string Text { get; set; }
        [Required]
        public string RecipeIngredients { get; set; }
    }
}