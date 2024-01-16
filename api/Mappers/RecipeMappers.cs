using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.RecipeDtos;
using api.Models;
using api.Models.HelperModels;
using Newtonsoft.Json;

namespace api.Mappers
{
    public static class RecipeMappers
    {
        public static Recipe toRecipeFromCreateDto(this CreateRecipe recipeDto)
        {
            return new Recipe
            {
                
                Title = recipeDto.Title,
                Text = recipeDto.Text,
                RecipeIngredients = JsonConvert.SerializeObject(recipeDto.RecipeIngredients, Formatting.None)
            };
        }

        public static GetRecipe toGetRecipeFromModel(this Recipe recipe)
        {
            return new GetRecipe
            {
                Title = recipe.Title,
                Text = recipe.Text,
                RecipeIngredients = JsonConvert.DeserializeObject<List<Ingredient>>(recipe.RecipeIngredients),
                Comments = recipe.Comments.Select(c => c.ToGetCommentFromModel()).ToList()
            };
        }
    }
}