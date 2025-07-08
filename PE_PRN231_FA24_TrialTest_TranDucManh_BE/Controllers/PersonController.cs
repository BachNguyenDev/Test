using BusinessLogicLayer.Services;
using DataAccessLayer.Dto;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace PE_PRN231_FA24_TrialTest_TranDucManh_BE.Controllers
{

    [Route("api")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IPersonService _personService;

        public PersonController(IPersonService personService)
        {
            _personService = personService;
        }

        [HttpPost("person")]
        public async Task<IActionResult> CreatePerson([FromBody] PersonCreateDto dto)
        {
            var validation = Validate(dto);
            if (validation != null)
                return BadRequest(new { error = validation });

            var person = new Person
            {
                PersonId = dto.PersonId,
                Fullname = dto.FullName,
                BirthDay = DateOnly.FromDateTime(dto.BirthDay),
                Phone = dto.Phone
            };

            var viruses = dto.Viruses.Select(v => (v.VirusName, v.ResistanceRate));
            await _personService.AddAsync(person, viruses);

            return Created("", new
            {
                personId = dto.PersonId,
                message = "Person and viruses added successfully"
            });
        }

        private string? Validate(PersonCreateDto dto)
        {
            if (dto.BirthDay >= new DateTime(2007, 1, 1))
                return "Value for Birthday < 01-01-2007";

            if (!Regex.IsMatch(dto.Phone, @"^\+84989\d{6}$"))
                return "Phone number must be in the format +84989xxxxxx";

            if (!Regex.IsMatch(dto.FullName, @"^([A-Z][a-zA-Z0-9@#]*\s?)+$"))
                return "Each word of the Fullname must begin with the capital letter";

            foreach (var virus in dto.Viruses)
            {
                if (virus.ResistanceRate < 0 || virus.ResistanceRate > 1)
                    return "Resistance Rate must be between 0 and 1";
            }

            return null;
        }



        [HttpGet("person/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var person = await _personService.GetByIdAsync(id);
            if (person == null) return NotFound();

            return Ok(new
            {
                personId = person.PersonId,
                fullName = person.Fullname,
                birthDay = person.BirthDay.ToString("yyyy-MM-dd"),
                phone = person.Phone,
                viruses = person.PersonViruses.Select(v => new
                {
                    virusName = v.Virus?.VirusName,
                    resistanceRate = v.ResistanceRate
                })
            });
        }

        [HttpGet("persons")]
        public async Task<IActionResult> GetAll()
        {
            var people = await _personService.GetAllAsync();
            return Ok(people.Select(person => new
            {
                personId = person.PersonId,
                fullName = person.Fullname,
                birthDay = person.BirthDay.ToString("yyyy-MM-dd"),
                phone = person.Phone,
                viruses = person.PersonViruses.Select(v => new
                {
                    virusName = v.Virus?.VirusName,
                    resistanceRate = v.ResistanceRate
                })
            }));
        }

        [HttpPut("person/{id}")]
        public async Task<IActionResult> UpdatePerson(int id, [FromBody] PersonUpdateDto dto)
        {
            var validation = Validate(dto);
            if (validation != null)
                return BadRequest(new { error = validation });

            var person = new Person
            {
                PersonId = id,
                Fullname = dto.FullName,
                BirthDay = DateOnly.FromDateTime(dto.BirthDay),
                Phone = dto.Phone
            };

            var viruses = dto.Viruses.Select(v => (v.VirusName, v.ResistanceRate));
            await _personService.UpdateAsync(person, viruses);

            return Ok(new { message = "Person and viruses updated successfully" });
        }

        private string? Validate(PersonUpdateDto dto)
        {
            if (dto.BirthDay >= new DateTime(2007, 1, 1))
                return "Value for Birthday < 01-01-2007";

            if (!Regex.IsMatch(dto.Phone, @"^\+84989\d{6}$"))
                return "Phone number must be in the format +84989xxxxxx";

            if (!Regex.IsMatch(dto.FullName, @"^([A-Z][a-zA-Z0-9@#]*\s?)+$"))
                return "Each word of the Fullname must begin with the capital letter";

            foreach (var v in dto.Viruses)
            {
                if (v.ResistanceRate < 0 || v.ResistanceRate > 1)
                    return "Resistance Rate must be between 0 and 1";
            }

            return null;
        }


        [HttpDelete("person/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _personService.DeleteAsync(id);
            return Ok(new { message = "Person and related viruses deleted successfully" });
        }

        private string? Validate(Person person)
        {
            if (person.BirthDay >= new DateOnly(2007, 1, 1))
                return "Value for Birthday < 01-01-2007";

            if ((!Regex.IsMatch(person.Phone, @"^\+84\d{9}$")))
                return "Phone number must be in the format +84989xxxxxx";

            if (!Regex.IsMatch(person.Fullname, @"^([A-Z][a-zA-Z0-9@#]*\s?)+$"))
                return "Each word of the Fullname must begin with the capital letter";

            foreach (var pv in person.PersonViruses)
            {
                if (pv.ResistanceRate < 0 || pv.ResistanceRate > 1)
                    return "Resistance Rate: Must be between 0 and 1";
                if (string.IsNullOrWhiteSpace(pv.Virus?.VirusName))
                    return "Virus name is required";
            }

            return null;
        }
    }
}
