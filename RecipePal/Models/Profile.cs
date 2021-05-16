using RecipePal.Models.Abstractions;
using System.Collections.Generic;

namespace RecipePal.Models
{
    public class Profile : BaseEntity
    {
        public string UserName { get; set; }
        public string Email { get; set; }

        public IEnumerable<Cookbook> Cookbooks { get; set; }
    }
}
