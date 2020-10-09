using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecipeApplication.Services
{
    public interface IRecipesService
    {
        Task AddRecipeAsync(RecipeBusinessObject recipeBusinessObject);
        Task<List<RecipeBusinessObject>> GetAllRecipesAsync(string filter);
        Task<RecipeBusinessObject> GetRecipeByIdAsync(int id);
        Task UpdateRecipeAsync(RecipeBusinessObject recipeBusinessObject);
        Task DeleteRecipeByIdAsync(int id);
    }
}
