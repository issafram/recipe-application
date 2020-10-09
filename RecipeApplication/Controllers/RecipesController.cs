using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RecipeApplication.Models;
using RecipeApplication.Services;

namespace RecipeApplication.Controllers
{
    public class RecipesController : Controller
    {
        private readonly IRecipesService recipesService;

        public RecipesController(IRecipesService recipesService)
        {
            this.recipesService = recipesService;
        }

        // GET: Recipes
        public async Task<IActionResult> Index([FromQuery]string filter)
        {
            var recipeBusinessObjects = await this.recipesService.GetAllRecipesAsync(filter);
            var recipeViewModels = recipeBusinessObjects.Select(x=> ConvertToViewModel(x)).ToList();
            var indexViewModel = new IndexViewModel { Recipes = recipeViewModels, Filter = filter };
            return View(indexViewModel);
        }

        // GET: Recipes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipeBusinessObject = await this.recipesService.GetRecipeByIdAsync(id.Value);
            if (recipeBusinessObject == null)
            {
                return NotFound();
            }

            var viewModel = ConvertToViewModel(recipeBusinessObject);

            return View(viewModel);
        }

        // GET: Recipes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Recipes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,ImageUrl,Comment,Author,Ingredients,Instruction")] AddRecipeViewModel addRecipeModel)
        {
            if (ModelState.IsValid)
            {
                await this.recipesService.AddRecipeAsync(new RecipeBusinessObject
                {
                    Author = addRecipeModel.Author,
                    Title = addRecipeModel.Title,
                    Comment = addRecipeModel.Comment,
                    Ingredients = addRecipeModel.Ingredients,
                    Instruction = addRecipeModel.Instruction,
                    ImageUrl = addRecipeModel.ImageUrl
                });
                return RedirectToAction(nameof(Index));
            }
            return View(addRecipeModel);
        }

        // GET: Recipes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipeBusinessObject = await this.recipesService.GetRecipeByIdAsync(id.Value);
            if (recipeBusinessObject == null)
            {
                return NotFound();
            }
            return View(ConvertToViewModel(recipeBusinessObject));
        }

        // POST: Recipes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ImageUrl,Comment,Author,Ingredients,Instruction")] ViewRecipeViewModel recipe)
        {
            if (id != recipe.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await this.recipesService.UpdateRecipeAsync(ConvertToBusinessObject(recipe));
                return RedirectToAction(nameof(Index));
            }
            return View(recipe);
        }

        // GET: Recipes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipeBusinessObject = await this.recipesService.GetRecipeByIdAsync(id.Value);
            if (recipeBusinessObject == null)
            {
                return NotFound();
            }

            return View(ConvertToViewModel(recipeBusinessObject));
        }

        // POST: Recipes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await this.recipesService.DeleteRecipeByIdAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Route("api/recipes")]
        public async Task<IActionResult> GetRecipes([FromQuery] string filter)
        {
            var recipeBusinessObjects = await this.recipesService.GetAllRecipesAsync(filter);
            var recipeViewModels = recipeBusinessObjects.Select(x => ConvertToViewModel(x)).ToList();
            return this.Ok(recipeViewModels);
        }

        private async Task<bool> RecipeExists(int id)
        {
            var recipeBusinessModel = await this.recipesService.GetRecipeByIdAsync(id);
            return recipeBusinessModel != null;
        }

        private static ViewRecipeViewModel ConvertToViewModel(RecipeBusinessObject recipeBusinessObject)
        {
            return new ViewRecipeViewModel
            {
                Id = recipeBusinessObject.Id,
                Author = recipeBusinessObject.Author,
                Title = recipeBusinessObject.Title,
                Comment = recipeBusinessObject.Comment,
                Instruction = recipeBusinessObject.Instruction,
                Ingredients = recipeBusinessObject.Ingredients,
                ImageUrl = recipeBusinessObject.ImageUrl
            };
        }

        private static RecipeBusinessObject ConvertToBusinessObject(ViewRecipeViewModel recipeViewModel)
        {
            return new RecipeBusinessObject
            {
                Id = recipeViewModel.Id,
                Author = recipeViewModel.Author,
                Title = recipeViewModel.Title,
                Comment = recipeViewModel.Comment,
                Instruction = recipeViewModel.Instruction,
                Ingredients = recipeViewModel.Ingredients,
                ImageUrl = recipeViewModel.ImageUrl
            };
        }
    }
}
