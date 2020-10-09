using Microsoft.EntityFrameworkCore;
using RecipeApplication.DataAccess;
using RecipeApplication.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RecipeApplication.Services
{
    public class RecipesService : IRecipesService
    {
        private readonly RecipesContext recipesContext;

        public RecipesService(RecipesContext recipesContext)
        {
            this.recipesContext = recipesContext;
        }

        public async Task AddRecipeAsync(RecipeBusinessObject recipeBusinessObject)
        {
            var entity = ConvertToEntity(recipeBusinessObject);
            await this.recipesContext.Recipe.AddAsync(entity);
            await this.recipesContext.SaveChangesAsync();
        }

        public async Task<List<RecipeBusinessObject>> GetAllRecipesAsync(string filter)
        {
            Expression<Func<Recipe, bool>> expression;
            if (string.IsNullOrWhiteSpace(filter))
            {
                expression = x => true;
            }
            else
            {
                expression = x => x.Title.Contains(filter) || x.Author.Contains(filter) || x.Comment.Contains(filter) || x.Ingredients.Contains(filter);
            }

            var recipes = await this.recipesContext.Recipe.AsNoTracking().Where(expression).ToListAsync();
            var recipeBusinessObjects = recipes.Select(x => ConvertToBusinessObject(x)).ToList();
            return recipeBusinessObjects;
        }

        public async Task<RecipeBusinessObject> GetRecipeByIdAsync(int id)
        {
            var recipeEntity = await this.recipesContext.Recipe.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (recipeEntity == null)
            {
                return null;
            }

            var recipeBusinessObject = ConvertToBusinessObject(recipeEntity);
            return recipeBusinessObject;
        }

        public async Task UpdateRecipeAsync(RecipeBusinessObject recipeBusinessObject)
        {
            var entity = await this.recipesContext.Recipe.FirstOrDefaultAsync(x => x.Id == recipeBusinessObject.Id);
            entity.Author = recipeBusinessObject.Author;
            entity.Comment = recipeBusinessObject.Comment;
            entity.ImageUrl = recipeBusinessObject.ImageUrl;
            entity.Ingredients = recipeBusinessObject.Ingredients;
            entity.Instruction = recipeBusinessObject.Instruction;
            entity.Title = recipeBusinessObject.Title;

            await this.recipesContext.SaveChangesAsync();
        }

        public async Task DeleteRecipeByIdAsync(int id)
        {
            var entity = await this.recipesContext.Recipe.FirstOrDefaultAsync(x => x.Id == id);
            this.recipesContext.Recipe.Remove(entity);
            await this.recipesContext.SaveChangesAsync();
        }

        private static RecipeBusinessObject ConvertToBusinessObject(Recipe recipeEntity)
        {
            return new RecipeBusinessObject 
            { 
                Id = recipeEntity.Id, 
                Author = recipeEntity.Author, 
                Comment = recipeEntity.Comment, 
                ImageUrl = recipeEntity.ImageUrl,
                Ingredients = recipeEntity.Ingredients,
                Instruction = recipeEntity.Instruction, 
                Title = recipeEntity.Title 
            };
        }

        private static Recipe ConvertToEntity(RecipeBusinessObject recipeBusinessObject)
        {
            return new Recipe 
            { 
                Id = recipeBusinessObject.Id, 
                Author = recipeBusinessObject.Author, 
                Comment = recipeBusinessObject.Comment, 
                ImageUrl = recipeBusinessObject.ImageUrl, 
                Ingredients = recipeBusinessObject.Ingredients,
                Instruction = recipeBusinessObject.Instruction, 
                Title = recipeBusinessObject.Title 
            };
        }
    }
}
