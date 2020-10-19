using Data.Contracts.Repositories;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Services.Mapping;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Data
{
    public class ContactsService : IContactsService
    {
        private readonly IDeletableEntityRepository<ContactLetter> repository;

        public ContactsService(IDeletableEntityRepository<ContactLetter> repository)
        {
            this.repository = repository;
        }

        public async Task<ContactLetter> CreateAsync(string senderName, string senderEmail, string about, string desc, string otherContacts, ApplicationUser user)
        {
            var letter = new ContactLetter
            {
                About = about,
                Description = desc,
                SenderName = senderName,
                SenderEmail = senderEmail,
                OtherContactInfo = otherContacts,
                User = user,
                IsPinned = false,
                IsViewed=false
            };

            await repository.AddAsync(letter);
            await repository.SaveChangesAsync();

            return letter;
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>()
        {
            return await repository.All().
                OrderByDescending(x => x.IsPinned).
                ThenBy(x => x.IsViewed).
                ThenByDescending(x => x.CreatedOn).
                To<T>().
                ToListAsync();
        }

        public async Task<T> GetByIdAsync<T>(string id)
        {
            return await repository.All().
                Where(x => x.Id == id).
                To<T>().
                FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(string id, bool isDeleted, bool isPinned, bool isViewed)
        {
            var letter = await repository.GetByIdWithDeletedAsync(id);
            if(letter == null)
            {
                return;
            }

            letter.IsPinned = isPinned;
            letter.IsViewed = isViewed;
            if(isDeleted)
            {
                repository.Delete(letter);
            }

            await repository.SaveChangesAsync();
        }
    }
}
