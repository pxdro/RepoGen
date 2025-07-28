using Microsoft.EntityFrameworkCore;
using RepoGen.Interface.EntitiesSlices.PermissionForm;
using FluentAssertions;

namespace RepoGenTest.Unit.EntitiesHandlers.Commands
{
    public class UpdatePermissionCommandHandlerTest
    {
        [Fact]
        public async Task Should_UpdatePermission_IfExistent_NoReturn()
        {
            // Arrange
            var factory = FakeDbContextFactory.Create();
            var context = factory.CreateDbContext();

            var existentPermission = await context.Permissions.AsNoTracking().FirstAsync();
            existentPermission.Name = "OtherReport";

            var command = new UpdatePermissionCommand(
                existentPermission.Id,
                existentPermission.Name
            );
            var handler = new UpdatePermissionCommandHandler(context);

            // Act
            await handler.Handle(command, default);

            // Assert
            var permissionAtualizado = await context.Permissions.AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == existentPermission.Id);
            permissionAtualizado.Should().NotBeNull();
            permissionAtualizado.Name.Should().Be(existentPermission.Name);
        }

        [Fact]
        public async Task Should_NotUpdatePermission_BecauseNoExists_ThrowsException()
        {
            // Arrange
            var factory = FakeDbContextFactory.Create();
            var context = factory.CreateDbContext();

            var command = new UpdatePermissionCommand(Guid.NewGuid(), string.Empty);
            var handler = new UpdatePermissionCommandHandler(context);

            // Act
            Func<Task> act = async () => await handler.Handle(command, default);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Permission not found");
        }
    }
}
