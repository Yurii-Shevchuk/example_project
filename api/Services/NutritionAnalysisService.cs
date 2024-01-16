using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.NutritionalDtos;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace api.Services
{
    public class NutritionAnalysisService : INutritionAnalysis
    {
        private readonly IRecipeRepository _recipeRepo;
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _httpClient;
        private readonly AppDbContext _context;

        public NutritionAnalysisService(IRecipeRepository recipeRepo, IConfiguration config, IHttpClientFactory httpClient, AppDbContext context)
        {
            _recipeRepo = recipeRepo;
            _httpClient = httpClient;
            _context = context;
            _config = config;
        }

        public async Task<ResponseNutritionalData> Create(int id)
        {
            var recipeToAnalyze = await _recipeRepo.GetById(id);
            var existingRecipe = await GetByRecipeId(id);
            if(recipeToAnalyze is null)
            {
                return null;
            }
            if(existingRecipe is not null)
            {
                return existingRecipe;
            }
            var recipeIngredients = recipeToAnalyze.Ingredients.ToIngredientsFromProcessing();
            var requestBody = new RequestBodyNutritionalDto
            {
                Title = recipeToAnalyze.Title,
                Ingredients = recipeIngredients
            };
            string jsonBody = JsonConvert.SerializeObject(requestBody);


            var nutritionalData = await GetNutritionalAnalysis(jsonBody);

            if(nutritionalData is null)
            {
                return null;
            }
            CreateNutritionalEntityDto nutritionalDataEntity = new CreateNutritionalEntityDto
            {
                RecipeId = id,
                Data = nutritionalData
            };
            var nutritionalModel = nutritionalDataEntity.ToNutritionalEntityFromDto();
            await _context.Nutrients.AddAsync(nutritionalModel);
            await _context.SaveChangesAsync();
            
             var formattedNutrients = JsonConvert.DeserializeObject<ResponseNutritionalData>(nutritionalModel.Data);

            return formattedNutrients;

        }
        private async Task<string> GetNutritionalAnalysis(string requestBody)
        {
             var queryParams = new Dictionary<string, string>()
            {
                {"app_id", $"{_config["ApiKeys:AppId"]}"},
                {"app_key", $"{_config["ApiKeys:AppKey"]}"},              
            };
            var httpClient = _httpClient.CreateClient("Edamam");
            var absoluteUrl = BuildUrl(httpClient.BaseAddress.ToString(), queryParams);

            StringContent content = new StringContent(requestBody, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(absoluteUrl, content);

            if(response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                return null;
            }
        }

        private static string BuildUrl(string baseUrl, Dictionary<string, string?> queryParams)
        {
            var queryString = QueryString.Create(queryParams);

            return baseUrl + queryString;
        } 

        public async Task<ResponseNutritionalData> GetByRecipeId(int id)
        {
            var recipeToAnalyze = await _context.Nutrients.Where(n => n.RecipeId == id).FirstOrDefaultAsync();
            if(recipeToAnalyze is null)
            {
                return null;
            }

            var formattedNutrients = JsonConvert.DeserializeObject<ResponseNutritionalData>(recipeToAnalyze.Data);

            return formattedNutrients;
        }
    }
}