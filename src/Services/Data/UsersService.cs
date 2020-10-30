using Data.Contracts.Repositories;
using Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Services.Contracts.Data;
using Services.Mapping;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Data
{
    public class UsersService : IUsersService
    {
        private readonly IRepository<ApplicationUser> repository;

        public UsersService(IRepository<ApplicationUser> repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<T>> GetUsersAsync<T>()
        {
            return await this.repository.AllAsNoTracking().
                To<T>().
                ToListAsync();
        }

        public async Task<T> GetUserByIdAsync<T>(string id)
        {
            return await this.repository.AllAsNoTracking().
                Where(x => x.Id == id).
                To<T>().
                FirstOrDefaultAsync();
        }
    }
}
