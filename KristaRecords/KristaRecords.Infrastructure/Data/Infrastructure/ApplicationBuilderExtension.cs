using KristaRecords.Infrastructure.Data.Domain;
using KristaRecords.Infrastrucutre.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KristaRecords.Infrastructure.Data.Infrastructure
{
    public static class ApplicationBuilderExtension
    {
        public static async Task<IApplicationBuilder> PrepareDatabase(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope()) 
            { 
                var services = serviceScope.ServiceProvider;
                await RoleSeeder(services);
                await SeedAdmin(services);

                var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                await SeedCategories(context);
            }

            return app;
        }

        private static async Task RoleSeeder(IServiceProvider serviceProvider)
        {
            var roleManagerContext = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roleNames = { "Administrator", "Client" };

            IdentityResult roleResult;

            foreach (var role in roleNames) 
            {
                var roleExist = await roleManagerContext.RoleExistsAsync(role);

                if(!roleExist)
                {
                    roleResult = await roleManagerContext.CreateAsync(new IdentityRole(role));
                }
            }
        }

        private static async Task SeedAdmin(IServiceProvider serviceProvider)
        {
            var userManagerContext = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            if(await userManagerContext.FindByNameAsync("admin") == null)
            {
                ApplicationUser user = new ApplicationUser();
                user.FirstName = "admin";
                user.LastName = "admin";
                user.UserName = "admin";
                user.Email = "admin@admin.com";
                user.Address = "Admin address";
                user.PhoneNumber = "0888888888";

                var result = await userManagerContext.CreateAsync(user, "Admin123456");

                if (result.Succeeded)
                {
                    await userManagerContext.AddToRoleAsync(user, "Administrator");
                }
            }
        }

        private static async Task SeedCategories(ApplicationDbContext context)
        {
            if (context.Categories.Any())
            {
                return;
            }
            context.Categories.AddRange(new[]
            {
                new Category { CategoryName = "Vocal Recording", HourlyRate = 35},
                new Category { CategoryName = "Music Recording", HourlyRate = 50},
                new Category { CategoryName = "Audio Dubbing & Voiceover", HourlyRate = 40},
                new Category { CategoryName = "Mastering", HourlyRate = 60},
                new Category { CategoryName = "Sound Mixing", HourlyRate = 30},
                new Category { CategoryName = "Video Recording", HourlyRate = 70},
                new Category {CategoryName = "Video + Sound Recording", HourlyRate = 120}
            });

            await context.SaveChangesAsync();
        }
    }
}
