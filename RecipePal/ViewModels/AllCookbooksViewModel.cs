using RecipePal.Models;
using System.Collections.Generic;

namespace RecipePal.ViewModels
{
    public class AllCookbooksViewModel
    {
        public IEnumerable<Cookbook> Cookbooks { get; set; }
    }
}
