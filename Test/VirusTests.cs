using Xunit;
using DataAccessLayer.Entities;
using System.Collections.Generic;

public class VirusTests
{
    [Fact]
    public void CanCreateVirus_WithValidProperties()
    {
        var virus = new Virus
        {
            VirusId = 1,
            VirusName = "Covid",
            Treatment = "Rest",
            PersonViruses = new List<PersonVirus>()
        };
        Assert.Equal(1, virus.VirusId);
        Assert.Equal("Covid", virus.VirusName);
        Assert.Equal("Rest", virus.Treatment);
        Assert.Empty(virus.PersonViruses);
    }
} 