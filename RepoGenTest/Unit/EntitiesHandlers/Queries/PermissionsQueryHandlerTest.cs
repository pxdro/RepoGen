using RepoGen.Interface.EntitiesSlices.Permissions;
using FluentAssertions;

namespace RepoGenTest.Unit.EntitiesHandlers.Queries
{
    public class PermissionsQueryHandlerTest
    {
        [Fact]
        internal async Task Should_GetPermissions_ReturnsPermissions()
        {
            // Arrange
            var fakeDbContextFactory = FakeDbContextFactory.Create();
            var fakeContext = fakeDbContextFactory.CreateDbContext();

            var command = new PermissionsQuery();
            var handler = new PermissionsQueryHandler(fakeContext);

            // Act
            var permissions = await handler.Handle(command, default);

            // Assert
            permissions.Should().NotBeNull();
            permissions.Count.Should().BeGreaterThan(0);
        }
    }
}
