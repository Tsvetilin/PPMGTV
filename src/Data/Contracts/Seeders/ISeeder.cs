using System.Threading.Tasks;

namespace Data.Contracts.Seeders
{
    public interface ISeeder
    {
        virtual int Priority { get { return 100; } }
        
        Task SeedAsync(ApplicationDbContext dbContext);
    }
}
