using RecipePal.Models.Abstractions;
using System.Collections.Generic;

namespace RecipePal.Models
{
    public class Cookbook : SocialElement
    {
        public int OwnerProfileId { get; set; }

        public string Title { get; set; }
        public string CuisineType { get; set; }

        public ICollection<Recipe> Recipes { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public Profile OwnerProfile { get; set; }
    }
}
