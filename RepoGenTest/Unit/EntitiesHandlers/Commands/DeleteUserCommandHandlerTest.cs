using Microsoft.EntityFrameworkCore;
using RepoGen.Interface.EntitiesSlices.Users;
using FluentAssertions;

namespace RepoGenTest.Unit.EntitiesHandlers.Commands
{
    public class DeleteUserCommandHandlerTest
    {
        [Fact]
        public async Task Should_DeleteUser_IfExistent_NoReturn()
        {
            // Arrange
            var factory = FakeDbContextFactory.Create();
            var context = factory.CreateDbContext();

            var existentUserId = (await context.Users.AsNoTracking().FirstAsync()).Id;

            var command = new DeleteUserCommand(existentUserId);
            var handler = new DeleteUserCommandHandler(context);

            // Act
            await handler.Handle(command, default);

            // Assert
            var user = await context.Users.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == existentUserId);
            user.Should().BeNull();
        }

        [Fact]
        public async Task Should_NotDeleteUser_IfNotExistent_NoReturn()
        {
            // Arrange
            var factory = FakeDbContextFactory.Create();
            var context = factory.CreateDbContext();

            var command = new DeleteUserCommand(Guid.NewGuid());
            var handler = new DeleteUserCommandHandler(context);

            // Act
            Func<Task> act = async () => await handler.Handle(command, default);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("User not found");
        }
    }
}
