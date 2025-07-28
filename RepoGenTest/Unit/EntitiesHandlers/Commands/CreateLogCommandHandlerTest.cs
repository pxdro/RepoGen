using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using RepoGen.Interface.ReportsSlices;
using RepoGen.Entities;
using FluentAssertions;
using Moq;

namespace RepoGenTest.Unit.EntitiesHandlers.Commands
{
    public class CreateLogCommandHandlerTest
    {
        [Fact]
        public async Task Should_CreateLog_NoReturn()
        {
            // Arrange
            var factory = FakeDbContextFactory.Create();
            var context = factory.CreateDbContext();
            var mockHttpContext = new Mock<IHttpContextAccessor>();

            var newLog = new LogContent
            {
                User = "testuser",
                Report = "Test Report"
            };
            var claims = new List<Claim>
            {
                new(ClaimTypes.GivenName, newLog.User)
            };

            var identity = new ClaimsIdentity(claims, "TestAuth");
            var principal = new ClaimsPrincipal(identity);

            var httpContext = new DefaultHttpContext { User = principal };
            mockHttpContext.Setup(h => h.HttpContext).Returns(httpContext);

            var handler = new CreateLogCommandHandler(context, mockHttpContext.Object);
            var command = new CreateLogCommand(newLog.Report);

            // Act
            await handler.Handle(command, default);

            // Assert
            var logCreated = await context.Logs.AsNoTracking()
                .FirstOrDefaultAsync(l => l.User == newLog.User);
            logCreated.Should().NotBeNull();
            logCreated.Report.Should().Be(newLog.Report);
        }
    }
}
