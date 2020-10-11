using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Contracts.Data
{
    public interface ITestsService
    {
        public Task<IEnumerable<T>> GetAll<T>();
        public Task<T> GetSingle<T>(string id);
    }
}
