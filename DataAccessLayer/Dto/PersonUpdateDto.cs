using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Dto
{
    public class PersonUpdateDto
    {
        public string FullName { get; set; } = string.Empty;
        public DateTime BirthDay { get; set; }
        public string Phone { get; set; } = string.Empty;
        public List<VirusDto> Viruses { get; set; } = new();
    }
}
