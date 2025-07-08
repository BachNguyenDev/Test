using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PE_PRN231_FA24_TrialTest_TranDucManh_BE.Controllers;
using DataAccessLayer.Entities;
using System.Collections.Generic;
using System.Linq;

public class AuthControllerTests
{
    private readonly Mock<ViroCureFal2024dbContext> _contextMock = new();
    private readonly Mock<IConfiguration> _configMock = new();
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        _controller = new AuthController(_contextMock.Object, _configMock.Object);
    }

    [Fact]
    public void Login_ReturnsBadRequest_WhenModelStateInvalid()
    {
        _controller.ModelState.AddModelError("Email", "Required");
        var result = _controller.Login(new LoginRequest());
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public void Login_ReturnsUnauthorized_WhenUserNotFound()
    {
        var users = new List<ViroCureUser>().AsQueryable();
        var mockSet = new Mock<Microsoft.EntityFrameworkCore.DbSet<ViroCureUser>>();
        mockSet.As<IQueryable<ViroCureUser>>().Setup(m => m.Provider).Returns(users.Provider);
        mockSet.As<IQueryable<ViroCureUser>>().Setup(m => m.Expression).Returns(users.Expression);
        mockSet.As<IQueryable<ViroCureUser>>().Setup(m => m.ElementType).Returns(users.ElementType);
        mockSet.As<IQueryable<ViroCureUser>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());
        _contextMock.Setup(c => c.ViroCureUsers).Returns(mockSet.Object);
        var login = new LoginRequest { Email = "a", Password = "b" };
        var result = _controller.Login(login);
        Assert.IsType<UnauthorizedObjectResult>(result);
    }
} 