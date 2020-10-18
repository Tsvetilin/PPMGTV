using Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Data
{
    public interface IContactsService
    {
        public Task<ContactLetter> CreateAsync(string senderName, string senderEmail, string about, string desc, string otherContacts, ApplicationUser user);
        public Task<IEnumerable<T>> GetAllAsync<T>();
        public Task<T> GetByIdAsync<T>(string id);

        public Task UpdateAsync(string id, bool isDeleted, bool isPinned, bool isViewed);
    }
}