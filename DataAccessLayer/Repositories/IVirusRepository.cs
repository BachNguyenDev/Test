using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DataAccessLayer.Repositories
{
    public interface IVirusRepository
    {
        Task<Virus?> GetByNameAsync(string virusName);
        Task<Virus> CreateAsync(Virus virus);
    }
}