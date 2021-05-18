using RecipePal.Models.Abstractions;

namespace RecipePal.Models
{
    public class Comment : SocialElement
    {
        public int OwnerProfileId { get; set; }
        public int CookbookId { get; set; }
        public string Text { get; set; }

        public Profile OwnerProfile { get; set; }
    }
}
