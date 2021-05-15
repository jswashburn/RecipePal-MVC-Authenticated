namespace RecipePal.Models
{
    public interface ISocialElement
    {
        int Likes { get; }
        int Dislikes { get; }
    }

    public abstract class SocialElement : BaseEntity, ISocialElement
    {
        public int Likes { get; set; }
        public int Dislikes { get; set; }
    }
}
