using api.Data;
using api.Dtos.RecipeDtos;
using api.Helpers;
using api.Models;
using api.Repositories;
using FakeItEasy;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace api.Tests;


public class RecipeRepositoryTests
{
    private async Task<AppDbContext> GetAppDbContextAsync()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var dbContext = new AppDbContext(options);
        dbContext.Database.EnsureCreated();
        if(await dbContext.Recipes.CountAsync() <= 0)
        {
            for(int i = 0; i < 10; i++)
            {
                dbContext.Recipes.Add(
                new Recipe
                {
                    Title = "Delicious Pasta Carbonara",
                    Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed ac purus vel nunc faucibus dapibus.",
                    RecipeIngredients = "200g spaghetti, 100g pancetta, 2 large eggs, 50g Pecorino cheese, Salt, Black pepper",
                    PostedOn = DateTime.Now,
                    Comments = null,  
                    NutritionalData = null 
                }
                );
            }
            await dbContext.SaveChangesAsync();
        }
        return dbContext;  
    }
    [Fact]
    public async void CreateRecipe_ReturnsRecipe()
    {
        var recipe = new Recipe();
        recipe.Title = "strawberry with banana";
        recipe.Text = "cook it";
        recipe.RecipeIngredients = $"[{{\"Name\": \"Bananas\", \"Quantity\": \"100\", \"Unit\": \"Grams\"}}, {{\"Name\": \"Strawberry\", \"Quantity\": \"100\", \"Unit\": \"Grams\"}}]";
        var dbContext = await GetAppDbContextAsync();
        var recipeRepository = new RecipeRepository(dbContext);

        var result = await recipeRepository.CreateAsync(recipe);

        result.Should().NotBeNull();
        result.Should().BeOfType<Recipe>();
    }

    [Fact]
    public async void DeleteAsync_ShouldReturnNull()
    {
        int id = 123;
        var dbContext = await GetAppDbContextAsync();
        var recipeRepository = new RecipeRepository(dbContext);

        var result = await recipeRepository.DeleteAsync(id);

        result.Should().BeNull();
    }

    [Fact]
    public async void DeleteAsync_ShouldReturnRecipe()
    {
        int id = 5;
        var dbContext = await GetAppDbContextAsync();
        var recipeRepository = new RecipeRepository(dbContext);

        var result = await recipeRepository.DeleteAsync(id);

        result.Should().BeOfType<Recipe>();
    }

    [Fact]
    public async void GetAll_ShouldReturnThreeRecipes()
    {
        var queryObject = new RecipeQueryObject();
        queryObject.PageSize = 3;
        var dbContext = await GetAppDbContextAsync();
        var recipeRepository = new RecipeRepository(dbContext);

        var result = await recipeRepository.GetAll(queryObject);

        result.Should().NotBeNullOrEmpty();
        result.Should().BeOfType<List<Recipe>>();
    }

    [Fact]
    public async void GetAll_ShouldReturnZeroRecipes()
    {
        var queryObject = new RecipeQueryObject();
        queryObject.PageNumber = 3;
        var dbContext = await GetAppDbContextAsync();
        var recipeRepository = new RecipeRepository(dbContext);

        var result = await recipeRepository.GetAll(queryObject);

        result.Should().BeNullOrEmpty();
        result.Should().BeOfType<List<Recipe>>();
    }

    [Fact]
    public async void Update_ShouldReturnRecipeAndBeDifferent()
    {
        int id = 5;
        var updatedRecipe = new UpdateRecipe{Title = "even cooler", Text = "now it's a cooler recipe", RecipeIngredients = "chocolate"};
        var dbContext = await GetAppDbContextAsync();
        var recipeRepository = new RecipeRepository(dbContext);
        var existingRecipe = await recipeRepository.GetById(id);
        var existingText = existingRecipe.Text ?? null;

        var result = await recipeRepository.Update(id, updatedRecipe);
        var textIsDifferent = result.Text != existingText;

        result.Should().BeOfType<Recipe>();
        textIsDifferent.Should().BeTrue();
    }
}