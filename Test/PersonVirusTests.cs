using Xunit;
using DataAccessLayer.Entities;

public class PersonVirusTests
{
    [Fact]
    public void CanCreatePersonVirus_WithValidProperties()
    {
        var person = new Person { PersonId = 1, Fullname = "A" };
        var virus = new Virus { VirusId = 2, VirusName = "Covid" };
        var pv = new PersonVirus
        {
            PersonId = 1,
            VirusId = 2,
            ResistanceRate = 0.5,
            Person = person,
            Virus = virus
        };
        Assert.Equal(1, pv.PersonId);
        Assert.Equal(2, pv.VirusId);
        Assert.Equal(0.5, pv.ResistanceRate);
        Assert.Equal(person, pv.Person);
        Assert.Equal(virus, pv.Virus);
    }
} 