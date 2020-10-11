using Common.Constants;
using Data.Contracts.Seeders;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Seeders
{
    public class RolesSeeder : ISeeder
    {
        public int Priority => 1;

        public async Task SeedAsync(ApplicationDbContext dbContext)
        {
            if (dbContext.Roles.Any())
            {
                return;
            }

            var rolesNames = new List<string>
            {
                ApplicationRolesNames.AdminRole,
            };

            foreach (var roleName in rolesNames)
            {
                await dbContext.Roles.AddAsync(new ApplicationRole
                {
                    Name = roleName,
                    NormalizedName = roleName.ToUpper(),
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                });
            }
        }
    }
}
