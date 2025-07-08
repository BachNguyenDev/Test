using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public interface IPersonService
    {
        Task<IEnumerable<Person>> GetAllAsync();
        Task<Person?> GetByIdAsync(int id);
        Task AddAsync(Person person, IEnumerable<(string virusName, double resistanceRate)> viruses);
        Task UpdateAsync(Person person, IEnumerable<(string virusName, double resistanceRate)> viruses);
        Task DeleteAsync(int id);
    }
}

