using RecipePal.Models.Abstractions;
using System.Collections.Generic;

namespace RecipePal.Models
{
    public class Recipe : SocialElement
    {
        public int CookbookId { get; set; }

        public string Title { get; set; }

        public ICollection<Note> Notes { get; set; }
        public Cookbook Cookbook { get; set; }
    }
}
