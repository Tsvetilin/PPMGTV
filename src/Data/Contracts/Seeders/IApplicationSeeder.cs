using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Data.Contracts.Seeders
{
    public interface IApplicationSeeder
    {
        Task SeedAsync(ApplicationDbContext dbContext, ILogger logger);
    }
}
