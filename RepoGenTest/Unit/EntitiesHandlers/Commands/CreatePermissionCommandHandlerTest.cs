using Microsoft.EntityFrameworkCore;
using RepoGen.Interface.EntitiesSlices.PermissionForm;
using FluentAssertions;

namespace RepoGenTest.Unit.EntitiesHandlers.Commands
{
    public class CreatePermissionCommandHandlerTest
    {
        [Fact]
        public async Task Should_CreatePermission_IfNotExistent_NoReturn()
        {
            // Arrange
            var factory = FakeDbContextFactory.Create();
            var context = factory.CreateDbContext();

            var handler = new CreatePermissionCommandHandler(context);
            var newPermission = "AnyReport";
            var command = new CreatePermissionCommand(newPermission);

            // Act
            await handler.Handle(command, default);

            // Assert
            var permissionCriada = await context.Permissions.AsNoTracking()
                .FirstOrDefaultAsync(p => p.Name == newPermission);

            permissionCriada.Should().NotBeNull();
            permissionCriada.Name.Should().Be(newPermission);
        }

        [Fact]
        public async Task Should_NotCreatePermission_BecauseExists_ThrowsException()
        {
            // Arrange
            var factory = FakeDbContextFactory.Create();
            var context = factory.CreateDbContext();

            var permissionExistent = (await context.Permissions.AsNoTracking().FirstAsync()).Name;

            var handler = new CreatePermissionCommandHandler(context);
            var command = new CreatePermissionCommand(permissionExistent);

            // Act
            Func<Task> act = async () => await handler.Handle(command, default);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Permission with this name already exists");
        }
    }
}

