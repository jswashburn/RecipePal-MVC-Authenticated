using System.ComponentModel.DataAnnotations;

namespace RecipePal.Models
{
    public class Note : BaseEntity
    {
        public int RecipeId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Text { get; set; }

        public Recipe Recipe { get; set; }
    }
}
