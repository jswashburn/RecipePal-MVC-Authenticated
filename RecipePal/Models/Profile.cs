using RecipePal.Models.Abstractions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RecipePal.Models
{
    public class Profile : BaseEntity
    {
        [Required]
        [RegularExpression(@"\w{6,20}")]
        public string UserName { get; set; }

        [Key]
        public string Email { get; set; }

        public IEnumerable<Cookbook> Cookbooks { get; set; }
    }
}
