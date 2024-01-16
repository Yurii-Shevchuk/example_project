using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.NutritionalDtos;
using api.Models;

namespace api.Interfaces
{
    public interface INutritionAnalysis
    {
        Task<ResponseNutritionalData> GetByRecipeId(int id);
        Task<ResponseNutritionalData> Create(int id);
    }
}