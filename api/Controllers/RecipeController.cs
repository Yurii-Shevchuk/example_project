using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.RecipeDtos;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace api.Controllers
{
    [Route("api/recipe")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly IRecipeRepository _recipeRepo;
        private readonly INutritionAnalysis _nutritionAnalysis;

        public RecipeController(IRecipeRepository recipeRepo, INutritionAnalysis nutritionAnalysis)
        {
            _nutritionAnalysis = nutritionAnalysis;
            _recipeRepo = recipeRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] RecipeQueryObject query)
        {
            var recipes = await _recipeRepo.GetAll(query);
            var recipeDto = recipes.Select(r => r.toGetRecipeFromModel());
            return Ok(recipeDto);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var recipe = await _recipeRepo.GetById(id);

            if(recipe is null)
            {
                return NotFound("Recipe not found");
            }

            var formattedRecipe = recipe.toGetRecipeFromModel();

            return Ok(formattedRecipe);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRecipe recipeDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var recipe = recipeDto.toRecipeFromCreateDto();

            await _recipeRepo.CreateAsync(recipe);

            return CreatedAtAction(nameof(GetById), new { id = recipe.RecipeId}, recipe.toGetRecipeFromModel());
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var recipeToDelete = await _recipeRepo.DeleteAsync(id);

            if(recipeToDelete is null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateRecipe updateDTO)
        {
            var recipeToUpdate = await _recipeRepo.Update(id, updateDTO);

            if(recipeToUpdate is null)
            {
                return NotFound();
            }

            return Ok(recipeToUpdate.toGetRecipeFromModel());
        }

        [HttpPost]
        [Route("{id:int}/analysis")]
        public async Task<IActionResult> ProcessRecipe([FromRoute] int id)
        {
            var nutritionalData = await _nutritionAnalysis.Create(id);

            if(nutritionalData is null)
            {
                return BadRequest("Your recipe could not be processed");
            }

            return Ok(nutritionalData);
        }

        [HttpGet("{id:int}/analysis")]
        public async Task<IActionResult> GetProcessedRecipe([FromRoute] int id)
        {
            var nutritionalData = await _nutritionAnalysis.GetByRecipeId(id);

            if(nutritionalData is null)
            {
                return NotFound("Nutritional data for the given recipe not found");
            }

            return Ok(nutritionalData);
        }

    }
}