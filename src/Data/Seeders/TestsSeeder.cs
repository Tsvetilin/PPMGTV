using Data.Contracts.Seeders;
using Data.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Seeders
{
    public class TestsSeeder : ISeeder
    {
        public int Priority => 3;

        public async Task SeedAsync(ApplicationDbContext dbContext)
        {
            if (dbContext.Users.Any())
            {
                return;
            }

            await dbContext.Tests.AddAsync(new TestModel
            {
                Name = "TestName",
                User = dbContext.Users.FirstOrDefault()
            });
        }
    }
}
