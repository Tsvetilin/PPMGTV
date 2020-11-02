using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Contracts.Data
{
    public interface IUsersService
    {
        public Task<IEnumerable<T>> GetUsersAsync<T>();

        public Task<T> GetUserByIdAsync<T>(string id);

        public string GetNameSuggestions(string part);

        public IEnumerable<T> GetSearchResultsUsers<T>(string part);
    }
}
