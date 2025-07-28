using Microsoft.EntityFrameworkCore;
using RepoGen.Interface.EntitiesSlices.PermissionForm;
using FluentAssertions;

namespace RepoGenTest.Unit.EntitiesHandlers.Queries
{
    public class PermissionQueryHandlerTest
    {
        [Fact]
        internal async Task Should_GetPermission_ReturnsPermission()
        {
            // Arrange
            var fakeDbContextFactory = FakeDbContextFactory.Create();
            var fakeContext = fakeDbContextFactory.CreateDbContext();
            var firstPermission = await fakeContext.Permissions.FirstAsync();

            var command = new PermissionQuery(firstPermission.Id);
            var handler = new PermissionQueryHandler(fakeContext);

            // Act
            var permission = await handler.Handle(command, default);

            // Assert
            permission.Should().NotBeNull();
            permission.Name.Should().Be(firstPermission.Name);
        }
    }
}
