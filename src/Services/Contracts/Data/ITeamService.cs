using Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Contracts.Data
{
    public interface ITeamService
    {
        public Task<IEnumerable<T>> GetAllAsync<T>();

        public Task<TeamMember> CreateAsync(
            ApplicationUser user,
            bool isActive,
            string photoUrl,
            string desc
            );

        public Task<T> GetTeamMemberByIdAsync<T>(string id);

        public Task<bool> DeleteAsync(string id);

        public Task UpdateAsync(
            string id,
            ApplicationUser user,
            bool isActive,
            string photoUrl,
            string desc);
        }
}
