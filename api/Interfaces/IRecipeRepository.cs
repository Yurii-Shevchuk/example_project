using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.RecipeDtos;
using api.Helpers;
using api.Models;

namespace api.Interfaces
{
    public interface IRecipeRepository
    {
        Task<List<Recipe>> GetAll(RecipeQueryObject query);
        Task<Recipe> GetById(int id);
        Task<Recipe> CreateAsync(Recipe recipe);
        Task<Recipe> Update(int id, UpdateRecipe updatedRecipe);
        Task<Recipe> DeleteAsync(int id);

    }
}