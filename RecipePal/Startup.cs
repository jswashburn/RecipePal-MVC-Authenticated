using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProfanityDetector.Interfaces;
using ProfanityDetector;
using RecipePal.Data;
using RecipePal.IdentityPolicy;
using RecipePal.Models.Identity;
using RecipePal.Repositories;
using RecipePal.Services;

namespace RecipePal
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddRazorPages();

            AddRecipePalServices(services);
            AddDbContexts(services);
            AddIdentity(services);
            ConfigureIdentityOptions(services);
        }

        #region Configuration Methods

        void AddRecipePalServices(IServiceCollection services)
        {
            services.AddScoped<IProfanityFilter, ProfanityFilter>();
            services.AddScoped<IRepositoryFactory, RepositoryFactory>();
            services.AddScoped<ICookbookBrowsingService, CookbookBrowsingService>();
            services.AddScoped<IRecipeBrowsingService, RecipeBrowsingService>();
        }

        void ConfigureIdentityOptions(IServiceCollection services)
        {
            services.Configure<IdentityOptions>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireDigit = true;
            });
        }

        void AddIdentity(IServiceCollection services)
        {
            services.AddTransient<IPasswordValidator<AppUser>, RecipePalPasswordPolicy>();
            services.AddTransient<IUserValidator<AppUser>, RecipePalUserPolicy>();

            services.AddIdentity<AppUser, IdentityRole>()
                    .AddEntityFrameworkStores<AppIdentityDbContext>()
                    .AddDefaultTokenProviders();
        }

        void AddDbContexts(IServiceCollection services)
        {
            services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("Identity"));
            });

            services.AddDbContext<RPDbContext>(options =>
            {
                options.UseInMemoryDatabase("RPDbContext");
            });
        }

        #endregion

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
