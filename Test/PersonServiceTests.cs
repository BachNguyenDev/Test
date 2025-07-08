using Xunit;
using Moq;
using BusinessLogicLayer.Services;
using DataAccessLayer.Repositories;
using DataAccessLayer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

public class PersonServiceTests
{
    private readonly Mock<IPersonRepository> _personRepoMock = new();
    private readonly Mock<IVirusRepository> _virusRepoMock = new();
    private readonly Mock<ViroCureFal2024dbContext> _contextMock = new();
    private readonly PersonService _service;

    public PersonServiceTests()
    {
        _service = new PersonService(_personRepoMock.Object, _virusRepoMock.Object, _contextMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllPersons()
    {
        var persons = new List<Person> { new Person { PersonId = 1, Fullname = "A" } };
        _personRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(persons);
        var result = await _service.GetAllAsync();
        Assert.Single(result);
        Assert.Equal("A", result.First().Fullname);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsPerson()
    {
        var person = new Person { PersonId = 1, Fullname = "A" };
        _personRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(person);
        var result = await _service.GetByIdAsync(1);
        Assert.NotNull(result);
        Assert.Equal(1, result.PersonId);
    }

    [Fact]
    public async Task AddAsync_AddsPersonWithViruses()
    {
        var person = new Person { PersonId = 1, Fullname = "A" };
        var viruses = new List<(string, double)> { ("Covid", 0.5) };
        _virusRepoMock.Setup(r => r.GetByNameAsync("Covid")).ReturnsAsync((Virus)null);
        _virusRepoMock.Setup(r => r.CreateAsync(It.IsAny<Virus>())).ReturnsAsync(new Virus { VirusId = 1, VirusName = "Covid" });
        _personRepoMock.Setup(r => r.AddAsync(person)).Returns(Task.CompletedTask);
        await _service.AddAsync(person, viruses);
        _personRepoMock.Verify(r => r.AddAsync(person), Times.Once);
        Assert.Single(person.PersonViruses);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesPersonAndViruses()
    {
        var person = new Person { PersonId = 1, Fullname = "A", PersonViruses = new List<PersonVirus>() };
        var existing = new Person { PersonId = 1, Fullname = "B", PersonViruses = new List<PersonVirus>() };
        var viruses = new List<(string, double)> { ("Covid", 0.5) };
        _personRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);
        _virusRepoMock.Setup(r => r.GetByNameAsync("Covid")).ReturnsAsync((Virus)null);
        _virusRepoMock.Setup(r => r.CreateAsync(It.IsAny<Virus>())).ReturnsAsync(new Virus { VirusId = 1, VirusName = "Covid" });
        _personRepoMock.Setup(r => r.UpdateAsync(existing)).Returns(Task.CompletedTask);
        _contextMock.Setup(c => c.PersonViruses.RemoveRange(existing.PersonViruses));
        await _service.UpdateAsync(person, viruses);
        _personRepoMock.Verify(r => r.UpdateAsync(existing), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_DeletesPerson()
    {
        _personRepoMock.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);
        await _service.DeleteAsync(1);
        _personRepoMock.Verify(r => r.DeleteAsync(1), Times.Once);
    }
} 