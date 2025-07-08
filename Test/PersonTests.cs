using Xunit;
using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;

public class PersonTests
{
    [Fact]
    public void CanCreatePerson_WithValidProperties()
    {
        var person = new Person
        {
            PersonId = 1,
            Fullname = "John Doe",
            BirthDay = new DateOnly(2000, 1, 1),
            Phone = "+84989123456",
            UserId = 2,
            PersonViruses = new List<PersonVirus>(),
            User = null
        };
        Assert.Equal(1, person.PersonId);
        Assert.Equal("John Doe", person.Fullname);
        Assert.Equal(new DateOnly(2000, 1, 1), person.BirthDay);
        Assert.Equal("+84989123456", person.Phone);
        Assert.Equal(2, person.UserId);
        Assert.Empty(person.PersonViruses);
        Assert.Null(person.User);
    }
} 