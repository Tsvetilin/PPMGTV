using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Contracts.Data
{
    public interface IUsersService
    {
        public Task<IEnumerable<T>> GetUsersAsync<T>();
        public Task<T> GetUserByIdAsync<T>(string id);
    }
}
