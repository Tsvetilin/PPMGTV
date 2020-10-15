using Common.Constants;
using Data.Contracts.Seeders;
using Data.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Seeders
{
    public class UsersSeeder : ISeeder
    {
        public int Priority => 2;

        public async Task SeedAsync(ApplicationDbContext dbContext)
        {
            if (dbContext.Users.Any())
            {
                return;
            }

            var username = "Admin";
            var email = "email@mail.com";
            var user = new ApplicationUser
            {
                Email = email,
                EmailConfirmed = true,
                NormalizedEmail = email.ToUpper(),
                NormalizedUserName = username.ToUpper(),
                UserName = username,
                FullName = $"{username} {username}"
            };

            var passwordHasher = new PasswordHasher<ApplicationUser>();
            user.PasswordHash = passwordHasher.HashPassword(user, "Pass");
            await dbContext.Users.AddAsync(user);

            await dbContext.UserRoles.AddAsync(new IdentityUserRole<string>()
            {
                UserId = user.Id,
                RoleId = dbContext.Roles.FirstOrDefault(x => x.Name == ApplicationRolesNames.AdminRole).Id
            });
        }
    }
}
