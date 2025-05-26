using Xunit;
using Moq;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using ZealandZooCase.Data;      // Your DbContext
using ZealandZooCase.Models;    // Your User class
using ZealandZooCase.Services;  // Where SetCurrentUser is defined

public class UserHelperServiceTests
{
    [Fact]
    public void SetCurrentUser_WithSessionUsername_ReturnsCorrectUser()
    {
        // Arrange

        // 1. Set up EF In-Memory DB with a known user
        var options = new DbContextOptionsBuilder<ZealandDBContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

        using var context = new ZealandDBContext(options);
        context.Users.Add(new User { UserId = 1, UserName = "testuser" });
        context.SaveChanges();

        // 2. Mock Session to return "testuser" when GetString("Username") is called
        var sessionMock = new Mock<ISession>();
        sessionMock.Setup(s => s.GetString("Username")).Returns("testuser");

        // 3. Mock HttpContext to return the session
        var httpContext = new DefaultHttpContext();
        httpContext.Session = sessionMock.Object;

        // 4. Mock IHttpContextAccessor to return the HttpContext
        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        httpContextAccessorMock.Setup(a => a.HttpContext).Returns(httpContext);

        // 5. Create the service with mocks
        var userService = new ZealandService(context, httpContextAccessorMock.Object);

        // Act
        var result = userService.SetCurrentUser();

        // Assert
        Assert.NotNull(result);
        Assert.Equal("testuser", result.UserName);
    }
}
