using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RecipePal.Models
{
    public class Chef : BaseEntity
    {
        [Required]
        [RegularExpression(@"\w{6,20}")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public ICollection<Cookbook> Cookbooks { get; set; }
    }
}
