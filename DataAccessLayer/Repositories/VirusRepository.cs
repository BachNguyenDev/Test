using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class VirusRepository : IVirusRepository
    {
        private readonly ViroCureFal2024dbContext _context;

        public VirusRepository(ViroCureFal2024dbContext context)
        {
            _context = context;
        }

        public async Task<Virus?> GetByNameAsync(string virusName)
        {
            return await _context.Viruses
                .FirstOrDefaultAsync(v => v.VirusName == virusName);
        }

        public async Task<Virus> CreateAsync(Virus virus)
        {
            await _context.Viruses.AddAsync(virus);
            await _context.SaveChangesAsync();
            return virus;
        }
    }
}