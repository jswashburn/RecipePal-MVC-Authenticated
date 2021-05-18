using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RecipePal.Models;
using RecipePal.Models.Identity;

namespace RecipePal.Data
{
    public class AppIdentityDbContext : IdentityDbContext<AppUser>
    {
        public DbSet<Cookbook> Cookbooks { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options) 
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) => modelBuilder.Seed();
    }
}
