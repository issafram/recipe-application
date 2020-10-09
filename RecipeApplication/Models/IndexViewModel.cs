using System.Collections.Generic;

namespace RecipeApplication.Models
{
    public class IndexViewModel
    {
        public string Filter { get; set; }
        public List<ViewRecipeViewModel> Recipes { get; set; }
    }
}