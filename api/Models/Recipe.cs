using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using api.Models.HelperModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace api.Models
{
    public class Recipe
    {
        public int RecipeId { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string RecipeIngredients {get; set;}
        public DateTime PostedOn { get; set; } = DateTime.Now;
        public List<Comment> Comments {get; set;} = new();
        public NutritionalData? NutritionalData { get; set; }
        [NotMapped]
        public List<Ingredient> Ingredients {
             get => JsonConvert.DeserializeObject<List<Ingredient>>(RecipeIngredients); 
             set => RecipeIngredients = JsonConvert.SerializeObject(value);
        }
    }
}