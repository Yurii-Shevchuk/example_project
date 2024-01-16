using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models.HelperModels;
using Newtonsoft.Json;

namespace api.Dtos.NutritionalDtos
{
    public class RequestBodyNutritionalDto
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("ingr")]
        public List<string> Ingredients { get; set; }
    }
}