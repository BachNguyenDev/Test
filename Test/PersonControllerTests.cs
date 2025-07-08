using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using PE_PRN231_FA24_TrialTest_TranDucManh_BE.Controllers;
using BusinessLogicLayer.Services;
using DataAccessLayer.Dto;
using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

public class PersonControllerTests
{
    private readonly Mock<IPersonService> _personServiceMock = new();
    private readonly PersonController _controller;

    public PersonControllerTests()
    {
        _controller = new PersonController(_personServiceMock.Object);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenPersonDoesNotExist()
    {
        _personServiceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync((Person)null);
        var result = await _controller.GetById(1);
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetById_ReturnsOk_WhenPersonExists()
    {
        var person = new Person { PersonId = 1, Fullname = "A", BirthDay = DateOnly.MinValue, Phone = "+84989123456", PersonViruses = new List<PersonVirus>() };
        _personServiceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(person);
        var result = await _controller.GetById(1);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task GetAll_ReturnsOk()
    {
        var people = new List<Person> { new Person { PersonId = 1, Fullname = "A", BirthDay = DateOnly.MinValue, Phone = "+84989123456", PersonViruses = new List<PersonVirus>() } };
        _personServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(people);
        var result = await _controller.GetAll();
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task CreatePerson_ReturnsBadRequest_WhenInvalid()
    {
        var dto = new PersonCreateDto { FullName = "a", Phone = "123", BirthDay = DateTime.Now, Viruses = new List<VirusDto>() };
        var result = await _controller.CreatePerson(dto);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task CreatePerson_ReturnsCreated_WhenValid()
    {
        var dto = new PersonCreateDto { PersonId = 1, FullName = "A", Phone = "+84989123456", BirthDay = new DateTime(2000,1,1), Viruses = new List<VirusDto>() };
        _personServiceMock.Setup(s => s.AddAsync(It.IsAny<Person>(), It.IsAny<IEnumerable<(string, double)>>())).Returns(Task.CompletedTask);
        var result = await _controller.CreatePerson(dto);
        Assert.IsType<CreatedResult>(result);
    }

    [Fact]
    public async Task UpdatePerson_ReturnsBadRequest_WhenInvalid()
    {
        var dto = new PersonUpdateDto { FullName = "a", Phone = "123", BirthDay = DateTime.Now, Viruses = new List<VirusDto>() };
        var result = await _controller.UpdatePerson(1, dto);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task UpdatePerson_ReturnsOk_WhenValid()
    {
        var dto = new PersonUpdateDto { FullName = "A", Phone = "+84989123456", BirthDay = new DateTime(2000,1,1), Viruses = new List<VirusDto>() };
        _personServiceMock.Setup(s => s.UpdateAsync(It.IsAny<Person>(), It.IsAny<IEnumerable<(string, double)>>())).Returns(Task.CompletedTask);
        var result = await _controller.UpdatePerson(1, dto);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Delete_ReturnsOk()
    {
        _personServiceMock.Setup(s => s.DeleteAsync(1)).Returns(Task.CompletedTask);
        var result = await _controller.Delete(1);
        Assert.IsType<OkObjectResult>(result);
    }
} 