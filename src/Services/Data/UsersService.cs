using Data.Contracts.Repositories;
using Data.Models;
using EllipticCurve;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Services.Contracts.Data;
using Services.Mapping;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
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

        public string GetNameSuggestions(string part)
        {
            List<string> names = new List<string>();
            var allNames = this.repository.AllAsNoTracking().Select(x => x.UserName.ToLower()).ToList();
            var search = part.ToLower();

            names.AddRange(allNames.Where(x => x.StartsWith(search)));

            for (int startIndex = 0; startIndex < search.Length; startIndex++)
            {
                for (int endIndex = 0; endIndex < search.Length; endIndex++)
                {
                    if (endIndex + 1 - startIndex > 0)
                    {
                        names.AddRange(allNames.Where(x => x.Contains(search.Substring(startIndex, endIndex + 1 - startIndex))));

                    }
                }
            }

            allNames = allNames.Distinct().ToList();

            var suggestions = allNames.
                Select(x =>
                    new Suggestion
                    {
                        Data = x,
                        Value = x
                    }).
                 ToArray();

            var suggestionResponse = new SuggestionResponse
            {
                Suggestions = suggestions
            };

            var response = JsonSerializer.Serialize(suggestionResponse);

            return response.ToLower();
        }

        private class SuggestionResponse
        {
            public Suggestion[] Suggestions { get; set; }
        }

        private class Suggestion
        {
            public string Value { get; set; }
            public string Data { get; set; }
        }
        /*
         "suggestions": [
        { "value": "United Arab Emirates", "data": "AE" },
        { "value": "United Kingdom",       "data": "UK" },
        { "value": "United States",        "data": "US" }
    ]
        */
    }
}
