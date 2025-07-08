using Xunit;
using DataAccessLayer.Repositories;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

public class PersonRepositoryTests
{
    // Các test cho PersonRepository sẽ cần mock DbContext và DbSet
    // Để đơn giản, chỉ minh họa test cho AddAsync và DeleteAsync
    [Fact]
    public async Task AddAsync_AddsPerson()
    {
        var mockSet = new Mock<DbSet<Person>>();
        var mockContext = new Mock<ViroCureFal2024dbContext>();
        mockContext.Setup(m => m.People).Returns(mockSet.Object);
        var repo = new PersonRepository(mockContext.Object);
        var person = new Person { PersonId = 1, Fullname = "A" };
        await repo.AddAsync(person);
        mockSet.Verify(m => m.AddAsync(person, default), Times.Once);
        mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once);
    }
} 