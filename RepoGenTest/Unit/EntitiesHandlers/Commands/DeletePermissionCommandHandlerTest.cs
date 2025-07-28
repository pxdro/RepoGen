using Microsoft.EntityFrameworkCore;
using RepoGen.Interface.EntitiesSlices.Permissions;
using FluentAssertions;

namespace RepoGenTest.Unit.EntitiesHandlers.Commands
{
    public class DeletePermissionCommandHandlerTest
    {
        [Fact]
        public async Task Should_DeletePermission_IfExistent_NoReturn()
        {
            // Arrange
            var factory = FakeDbContextFactory.Create();
            var context = factory.CreateDbContext();

            var existentPermissionId = (await context.Permissions.AsNoTracking()
                .FirstOrDefaultAsync(p => p.Name.Equals("PermissionNotIncluded")))!.Id;

            var command = new DeletePermissionCommand(existentPermissionId);
            var handler = new DeletePermissionCommandHandler(context);

            // Act
            await handler.Handle(command, default);

            // Assert
            var permission = await context.Permissions.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == existentPermissionId);
            permission.Should().BeNull();
        }

        [Fact]
        public async Task Should_NotDeletePermission_IfNotExistent_NoReturn()
        {
            // Arrange
            var factory = FakeDbContextFactory.Create();
            var context = factory.CreateDbContext();

            var command = new DeletePermissionCommand(Guid.NewGuid());
            var handler = new DeletePermissionCommandHandler(context);

            // Act
            Func<Task> act = async () => await handler.Handle(command, default);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Permission not found");
        }
    }
}
