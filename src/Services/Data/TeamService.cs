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
        private readonly IDeletableEntityRepository<TeamMember> repository;

        public TeamService(IDeletableEntityRepository<TeamMember> repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>()
        {
            return await this.repository.AllAsNoTracking().
                OrderBy(x => x.IsActiveMember).
                ThenBy(x => x.CreatedOn).
                To<T>().
                ToListAsync();
        }

        public async Task<TeamMember> CreateAsync(
            ApplicationUser user,
            bool isActive,
            string photoUrl,
            string desc
            )
        {
            var member = new TeamMember
            {
                IsActiveMember = isActive,
                User = user,
                PhotoUrl = photoUrl,
                Descrtiption = desc
            };

            await this.repository.AddAsync(member);
            await this.repository.SaveChangesAsync();

            return member;
        }

        public async Task<T> GetTeamMemberByIdAsync<T>(string id)
        {
            return await this.repository.AllAsNoTracking().
                Where(x => x.Id == id).
                To<T>().
                FirstOrDefaultAsync();
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var member = await this.repository.GetByIdWithDeletedAsync(id);
            if (member == null)
            {
                return false;
            }

            this.repository.Delete(member);
            await repository.SaveChangesAsync();
            return true;
        }

        public async Task UpdateAsync(
            string id,
            ApplicationUser user,
            bool isActive,
            string photoUrl,
            string desc)
        {
            var member = new TeamMember
            {
                Id = id,
                IsActiveMember = isActive,
                User = user,
                PhotoUrl = photoUrl,
                Descrtiption = desc
            };

            this.repository.Update(member);
            await repository.SaveChangesAsync();
        }
    }
}
