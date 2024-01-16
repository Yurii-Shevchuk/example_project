using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace api.Dtos.NutritionalDtos
{
    public class ResponseNutritionalData
    {
        [JsonProperty("totalWeight")]
        public double TotalWeight { get; set; }
        [JsonProperty("totalNutrientsKCal")]
        public Dictionary<string, Nutrient> TotalNutrients { get; set; }
    }

    public class Nutrient
    {
        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("unit")]
        public string Unit { get; set; }
    }
}