using System;

namespace RecipePal.Models
{
    public interface IEntity
    {
        public int Id { get; }
        public DateTime CreatedAt { get; }
    }

    public class BaseEntity : IEntity
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
