namespace RecipeApplication.Models
{
    public class ViewRecipeViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string Comment { get; set; }
        public string Author { get; set; }
        public string Instruction { get; set; }
        public string Ingredients { get; set; }
    }
}
