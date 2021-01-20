using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Data
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        { 
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();

            //Configure default provider and connection string
            builder.UseSqlServer("CONNECTION STRING");

            return new ApplicationDbContext(builder.Options);
        }
    }
}