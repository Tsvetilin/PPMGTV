using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Contracts.Data
{
    public interface IVideosService
    {
        public Task<T> GetLatestVideo<T>();
        public double CountAllFilms();
        public Task<IEnumerable<T>> GetVideosOnPage<T>(int currentPage, int videosOnPage);
    }
}
