using RecipePal.Models;
using System.Collections.Generic;

namespace RecipePal.ViewModels
{
    public class AllCookbooksViewModel
    {
        public bool CanEdit { get; set; }
        public IEnumerable<Cookbook> Cookbooks { get; set; }

        //public List<Cookbook> GetSorted()
        //{

        //}
    }
}
