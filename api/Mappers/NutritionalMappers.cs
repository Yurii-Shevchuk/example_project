using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.NutritionalDtos;
using api.Models;
using api.Models.HelperModels;

namespace api.Mappers
{
    public static class NutritionalMappers
    {
        public static NutritionalData ToNutritionalEntityFromDto(this CreateNutritionalEntityDto entityDto)
        {
            return new NutritionalData
            {
                RecipeId = entityDto.RecipeId,
                Data = entityDto.Data,
            };
        }

        public static List<string> ToIngredientsFromProcessing(this List<Ingredient> ingredients)
        {
            List<string> list = new();
            foreach (var ingredient in ingredients)
            {
                list.Add($"{ingredient.Name} {ingredient.Quantity} {ingredient.Unit}");
            }
            return list;
        }
    }
}