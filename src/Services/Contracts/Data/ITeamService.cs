﻿using Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Contracts.Data
{
    public interface ITeamService
    {
        public Task<IEnumerable<T>> GetAllAsync<T>();

        public Task<Team> CreateAsync(
            string title,
            string years,
            string photoUrl,
            string preDesc,
            string desc
            );

        public Task<T> GetTeamByIdAsync<T>(string id);

        public Task<bool> DeleteAsync(string id);

        public Task UpdateAsync(
            string id,
            string title,
            string years,
            string photoUrl,
            string preDesc,
            string desc
            );
    }
}
