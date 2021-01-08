using Common.Helpers;
using Data.Contracts.Repositories;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Services.Contracts.Data;
using Services.Mapping;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Data
{
    public class TeamService : ITeamService
    {
        private readonly IDeletableEntityRepository<Team> repository;

        public TeamService(IDeletableEntityRepository<Team> repository)
        {
            this.repository = repository;
        }

        public async Task<Team> CreateAsync(string title, string years, string photoUrl, string preDesc, string desc)
        {
            var team = new Team
            {
                TeamTitle=title,
                TeamYears=years,
                PhotoUrl=photoUrl,
                PreDescription=preDesc,
                Descrtiption=desc.SanitizeHtml(),
            };

            await this.repository.AddAsync(team);
            await this.repository.SaveChangesAsync();

            return team;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var team = await this.repository.GetByIdWithDeletedAsync(id);
            if (team == null)
            {
                return false;
            }

            this.repository.Delete(team);
            await repository.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>()
        {
            return await this.repository.AllAsNoTracking().
                OrderByDescending(x => x.CreatedOn).
                To<T>().
                ToListAsync();
        }

        public async Task<T> GetTeamByIdAsync<T>(string id)
        {
            return await this.repository.AllAsNoTracking().
                Where(x => x.Id == id).
                To<T>().
                FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(string id, string title, string years, string photoUrl, string preDesc, string desc)
        {
            var team = new Team
            {
                Id=id,
                TeamTitle = title,
                TeamYears = years,
                PhotoUrl = photoUrl,
                PreDescription = preDesc,
                Descrtiption = desc.SanitizeHtml(),
            };

            this.repository.Update(team);
            await repository.SaveChangesAsync();
        }
    }
}
