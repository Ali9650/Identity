using Identity.Data;
using Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace Identity
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

            builder.Services.AddIdentity<User, IdentityRole>(option =>
            {
                option.Password.RequiredLength = 8;
                option.Password.RequireUppercase = true;
                option.Password.RequireLowercase = false;
                option.Password.RequireDigit = true;
                option.User.RequireUniqueEmail = true;

            }).AddEntityFrameworkStores<AppDbContext>();

            var app = builder.Build();
			app.UseStaticFiles();

			app.UseAuthentication();
			app.UseAuthorization();
			app.MapControllerRoute(
            name: "default",
            pattern: "{controller=account}/{action=register}/{id?}"
            );


            using (var scope = app.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetService<UserManager<User>>();
                var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
                DbInitializer.SeedData(roleManager, userManager);
            }


            app.Run();
        }
    }
}
