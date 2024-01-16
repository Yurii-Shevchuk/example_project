using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.RecipeDtos;
using api.Helpers;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class RecipeRepository : IRecipeRepository
    {
        private readonly AppDbContext _context;
        public RecipeRepository(AppDbContext context)
        {
            _context = context;  
        }

        public async Task<Recipe> CreateAsync(Recipe recipe)
        {
            await _context.Recipes.AddAsync(recipe);
            await _context.SaveChangesAsync();
            return recipe;
        }

        public async Task<Recipe> DeleteAsync(int id)
        {
            var recipe = await _context.Recipes.FirstOrDefaultAsync(r => r.RecipeId == id);

            if(recipe is null)
            {
                return null;
            }

            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();
            return recipe;
        }

        public async Task<List<Recipe>> GetAll(RecipeQueryObject query)
        {
            var recipes = _context.Recipes.Include(c => c.Comments).AsQueryable();

            if(!string.IsNullOrWhiteSpace(query.Title))
            {
                recipes = recipes.Where(r => r.Title.ToLower().Contains(query.Title.ToLower()));
            }

            int recipesToSkip = (query.PageNumber - 1) * query.PageSize;

            return await recipes.Skip(recipesToSkip).Take(query.PageSize).ToListAsync();
        }

        public async Task<Recipe> GetById(int id)
        {
            return await _context.Recipes.Include(c => c.Comments).FirstOrDefaultAsync(i => i.RecipeId == id);
        }

        public async Task<Recipe> Update(int id, UpdateRecipe updatedRecipe)
        {
            var existingRecipe = await _context.Recipes.FirstOrDefaultAsync(r => r.RecipeId == id);

            if(existingRecipe is null)
            {
                return null;
            }

            existingRecipe.Title = updatedRecipe.Title;
            existingRecipe.Text = updatedRecipe.Text;
            existingRecipe.RecipeIngredients = updatedRecipe.RecipeIngredients;

            await _context.SaveChangesAsync();

            return existingRecipe;
        }
    }
}