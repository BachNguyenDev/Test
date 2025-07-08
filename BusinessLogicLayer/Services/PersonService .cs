using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _personRepo;
        private readonly IVirusRepository _virusRepo;
        private readonly ViroCureFal2024dbContext _context;

        public PersonService(IPersonRepository personRepo, IVirusRepository virusRepo, ViroCureFal2024dbContext context)
        {
            _personRepo = personRepo;
            _virusRepo = virusRepo;
            _context = context;
        }

        public async Task<IEnumerable<Person>> GetAllAsync()
        {
            return await _personRepo.GetAllAsync();
        }

        public async Task<Person?> GetByIdAsync(int id)
        {
            return await _personRepo.GetByIdAsync(id);
        }

        public async Task AddAsync(Person person, IEnumerable<(string virusName, double resistanceRate)> viruses)
        {
            foreach (var (virusName, resistanceRate) in viruses)
            {
                var virus = await _virusRepo.GetByNameAsync(virusName);
                if (virus == null)
                {
                    virus = await _virusRepo.CreateAsync(new Virus
                    {
                        VirusName = virusName,
                        Treatment = ""
                    });
                }

                person.PersonViruses.Add(new PersonVirus
                {
                    VirusId = virus.VirusId,
                    ResistanceRate = resistanceRate
                });
            }

            await _personRepo.AddAsync(person);
        }

        public async Task UpdateAsync(Person person, IEnumerable<(string virusName, double resistanceRate)> viruses)
        {
            var existingPerson = await _personRepo.GetByIdAsync(person.PersonId);
            if (existingPerson == null)
                throw new KeyNotFoundException("Person not found");

            existingPerson.Fullname = person.Fullname;
            existingPerson.BirthDay = person.BirthDay;
            existingPerson.Phone = person.Phone;

            _context.PersonViruses.RemoveRange(existingPerson.PersonViruses);

            foreach (var (virusName, resistanceRate) in viruses)
            {
                var virus = await _virusRepo.GetByNameAsync(virusName);
                if (virus == null)
                {
                    virus = await _virusRepo.CreateAsync(new Virus
                    {
                        VirusName = virusName,
                        Treatment = ""
                    });
                }

                existingPerson.PersonViruses.Add(new PersonVirus
                {
                    VirusId = virus.VirusId,
                    ResistanceRate = resistanceRate
                });
            }

            await _personRepo.UpdateAsync(existingPerson);
        }

        public async Task DeleteAsync(int id)
        {
            await _personRepo.DeleteAsync(id);
        }
    }
}