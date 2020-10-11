using Common;
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
    public class TestsService :ITestsService
    {
        private readonly IDeletableEntityRepository<TestModel> repository;

        public TestsService(IDeletableEntityRepository<TestModel> repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<T>> GetAll<T>()
        {
            var tests= await this.repository
              .All()
              .OrderBy(x => x.Name)
              .To<T>()
              .ToListAsync();
            return tests;
        }

        public async Task<T> GetSingle<T>(string id)
        {
            var result = await this.repository.All().Where(x => x.Id == id).To<T>().FirstOrDefaultAsync();
            return result;
        }
    }
}
