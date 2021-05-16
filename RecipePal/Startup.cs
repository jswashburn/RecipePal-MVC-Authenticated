using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RecipePal.Data;
using RecipePal.IdentityPolicy;
using RecipePal.Models.Identity;
using RecipePal.Repositories;
using RecipePal.Services;
using System;

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

            AddDbContexts(services);
            AddIdentity(services);
            AddRecipePalServices(services);
            ConfigureOptions(services);
            ConfigureCookies(services);
        }

        #region Configuration Methods

        void AddRecipePalServices(IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<ICookbookBrowsingService, CookbookBrowsingService>();
            services.AddScoped<IRecipeBrowsingService, RecipeBrowsingService>();
        }

        void ConfigureCookies(IServiceCollection services)
        {
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = ".AspNetCore.Identity.Application";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                options.SlidingExpiration = true;
            });
        }

        void ConfigureOptions(IServiceCollection services)
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
            //services.AddAuthentication().AddGoogle(options =>
            //{
            //    options.ClientId = Configuration["OAuth:Google:ClientID"];
            //    options.ClientSecret = Configuration["OAuth:Google:ClientSecret"];
            //    options.SignInScheme = IdentityConstants.ExternalScheme;
            //});

            services.AddTransient<IPasswordValidator<AppUser>, RecipePalPasswordPolicy>();
            services.AddTransient<IUserValidator<AppUser>, RecipePalUserPolicy>();

            services.AddIdentity<AppUser, IdentityRole>()
                    .AddEntityFrameworkStores<AppIdentityDbContext>()
                    .AddDefaultTokenProviders();
        }

        void AddDbContexts(IServiceCollection services)
        {
            services.AddDbContext<RPDbContext>(options =>
            {
                options.UseInMemoryDatabase("RPDbContext");
            });

            services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("Identity"));
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
