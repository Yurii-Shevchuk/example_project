using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace api.Controllers
{
    [Route("api/ingredient")]
    [ApiController]
    public class IngredientController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _httpClient;
        public IngredientController(IConfiguration config, IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
            _config = config;
            
        }
        // [HttpGet]
        // public async Task<IActionResult> GetIngredient(string ingredient)
        // {
        //     var queryParams = new Dictionary<string, string>()
        //     {
        //         {"app_id", $"{_config["ApiKeys:AppId"]}"},
        //         {"app_key", $"{_config["ApiKeys:AppKey"]}"},
        //         {"nutrition-type", "cooking"},
        //         {"ingr", ingredient}
                
        //     };
        //     var httpClient = _httpClient.CreateClient("Edamam");
        //     var absoluteUrl = BuildUrl(httpClient.BaseAddress.ToString(), queryParams);
        //     using (var request = new HttpRequestMessage(HttpMethod.Get, absoluteUrl))
        //     {
        //         request.Headers.Add("Accept", "application/json");
        //         var httpResponse = await httpClient.SendAsync(request);
            
        //     //var httpResponse = await httpClient.GetAsync(absoluteUrl);

        //     if (httpResponse.IsSuccessStatusCode)
        //     {
        //         var contentString = await httpResponse.Content.ReadFromJsonAsync<NutrientsResponseDto>();
        //         //var calories = FromJson(contentString);
        //         return Ok(contentString);
                
        //     }
        //     }
        //     return BadRequest("Could not process the ingredient");
        // }

        private static string BuildUrl(string baseUrl, Dictionary<string, string?> queryParams)
        {
            var queryString = QueryString.Create(queryParams);

            return baseUrl + queryString;
        } 
        
    }
}