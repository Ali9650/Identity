using Identity.Constants.Enums;
using Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace Identity
{
    public static class DbInitializer
    {
        public static void SeedData(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            AddRoles(roleManager);
            AddAdmin(userManager, roleManager);
        }

        private static void AddRoles(RoleManager<IdentityRole> roleManager)
        {
            foreach (var role in Enum.GetValues<UserRoles>())
            {
                if (!roleManager.RoleExistsAsync(role.ToString()).Result)
                {
                    _ = roleManager.CreateAsync(new IdentityRole
                    {
                        Name = role.ToString(),
                    }).Result;
                }
            }
        }
        private static void AddAdmin(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (userManager.FindByEmailAsync("Admin@app.com").Result is null)
            {

                var user = new User
                {
                    UserName = "Admin@app.com",
                    Country = "Azerbaijan",
                    Email = "Admin@app.com",
                    City = "Baku",
                    IsSubscribe = true
                };

                var result = userManager.CreateAsync(user, "Admin12345!").Result;
                if (!result.Succeeded) throw new Exception("admin yaratmaq mumkun olmadi");

                var role = roleManager.FindByNameAsync("Admin").Result;
                if (role?.Name is null) throw new Exception("uygun rol tapilmadi");


                var addToRoleResult = userManager.AddToRoleAsync(user, role.Name).Result;
                if (!addToRoleResult.Succeeded) throw new Exception("admin rolunu elave etmek mumkun olmadi");
            }

        }
    }
}