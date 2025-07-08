using Xunit;
using DataAccessLayer.Entities;

public class ViroCureUserTests
{
    [Fact]
    public void CanCreateViroCureUser_WithValidProperties()
    {
        var user = new ViroCureUser
        {
            UserId = 1,
            Email = "test@example.com",
            Password = "123456",
            Role = 1
        };
        Assert.Equal(1, user.UserId);
        Assert.Equal("test@example.com", user.Email);
        Assert.Equal("123456", user.Password);
        Assert.Equal(1, user.Role);
    }
} 