using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Controllers;
using api.Dtos.RecipeDtos;
using api.Helpers;
using api.Interfaces;
using api.Models;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace api.Tests
{
    public class RecipeControllerTests
    {
        private readonly IRecipeRepository _recipeRepo;
        private readonly INutritionAnalysis _nutritionAnalysis;

        public RecipeControllerTests()
        {
            _recipeRepo = A.Fake<IRecipeRepository>();
            _nutritionAnalysis = A.Fake<INutritionAnalysis>();
        }

        [Fact]
        public async void GetAllRecipes_ReturnsOk()
        {
            var queryObject = new RecipeQueryObject();
            var controller = new RecipeController(_recipeRepo, _nutritionAnalysis);

            var result = await controller.GetAll(queryObject);

            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));
        }

        [Fact]
        public async void Delete_ReturnsNotFound()
        {
            int id = 5;
            Recipe recipe = null;
            var controller = new RecipeController(_recipeRepo, _nutritionAnalysis);
            A.CallTo(() => _recipeRepo.DeleteAsync(id)).Returns(recipe);

            var result = await controller.Delete(id);

            result.Should().BeOfType(typeof(NotFoundResult));
        }
    }
}