using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly ViroCureFal2024dbContext _context;

        public PersonRepository(ViroCureFal2024dbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Person>> GetAllAsync()
        {
            return await _context.People
                .Include(p => p.PersonViruses)
                .ThenInclude(pv => pv.Virus)
                .ToListAsync();
        }

        public async Task<Person?> GetByIdAsync(int id)
        {
            return await _context.People
                .Include(p => p.PersonViruses)
                .ThenInclude(pv => pv.Virus)
                .FirstOrDefaultAsync(p => p.PersonId == id);
        }

        public async Task AddAsync(Person person)
        {
            await _context.People.AddAsync(person);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Person person)
        {
            _context.People.Update(person);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var person = await _context.People
                .Include(p => p.PersonViruses)
                .FirstOrDefaultAsync(p => p.PersonId == id);

            if (person != null)
            {
                _context.PersonViruses.RemoveRange(person.PersonViruses);
                _context.People.Remove(person);
                await _context.SaveChangesAsync();
            }
        }

    }
}
