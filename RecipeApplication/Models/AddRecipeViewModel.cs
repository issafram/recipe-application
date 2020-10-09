namespace RecipeApplication.Models
{
    public class AddRecipeViewModel
    {
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string Comment { get; set; }
        public string Author { get; set; }
        public string Instruction { get; set; }
        public string Ingredients { get; set; }
    }
}