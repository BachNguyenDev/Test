using Xunit;
using DataAccessLayer.Entities;

public class LoginRequestTests
{
    [Fact]
    public void CanCreateLoginRequest_WithValidProperties()
    {
        var login = new LoginRequest
        {
            Email = "test@example.com",
            Password = "123456"
        };
        Assert.Equal("test@example.com", login.Email);
        Assert.Equal("123456", login.Password);
    }
} 