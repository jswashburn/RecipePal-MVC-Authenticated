using Microsoft.EntityFrameworkCore;
using RecipePal.Models;

namespace RecipePal.Data
{
    public class RPDbContext : DbContext
    {
        public DbSet<Cookbook> Cookbooks { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public RPDbContext(DbContextOptions<RPDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Seed();
        }
    }
}
