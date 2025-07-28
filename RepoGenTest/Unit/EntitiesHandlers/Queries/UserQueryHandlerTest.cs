using Microsoft.EntityFrameworkCore;
using RepoGen.Interface.EntitiesSlices.UserForm;
using FluentAssertions;

namespace RepoGenTest.Unit.EntitiesHandlers.Queries
{
    public class UserQueryHandlerTest
    {
        [Fact]
        internal async Task Should_GetUser_ReturnsUser()
        {
            // Arrange
            var fakeDbContextFactory = FakeDbContextFactory.Create();
            var fakeContext = fakeDbContextFactory.CreateDbContext();
            var firstUser = await fakeContext.Users.FirstAsync();

            var command = new UserQuery(firstUser.Id);
            var handler = new UserQueryHandler(fakeContext);

            // Act
            var user = await handler.Handle(command, default);

            // Assert
            user.Should().NotBeNull();
            user.Name.Should().Be(firstUser.Name);
        }
    }
}
