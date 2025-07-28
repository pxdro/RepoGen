using RepoGen.Interface.EntitiesSlices.Users;
using FluentAssertions;

namespace RepoGenTest.Unit.EntitiesHandlers.Queries
{
    public class UsersQueryHandlerTest
    {
        [Fact]
        internal async Task Should_GetUsers_ReturnsUsers()
        {
            // Arrange
            var fakeDbContextFactory = FakeDbContextFactory.Create();
            var fakeContext = fakeDbContextFactory.CreateDbContext();

            var command = new UsersQuery();
            var handler = new UsersQueryHandler(fakeContext);

            // Act
            var users = await handler.Handle(command, default);

            // Assert
            users.Should().NotBeNull();
            users.Count().Should().BeGreaterThan(0);
        }
    }
}
