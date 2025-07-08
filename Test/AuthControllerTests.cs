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
        _contextMock.Setup(c => c.ViroCureUsers).Returns(users);
        var login = new LoginRequest { Email = "a", Password = "b" };
        var result = _controller.Login(login);
        Assert.IsType<UnauthorizedObjectResult>(result);
    }
} 