using Microsoft.EntityFrameworkCore;
using RepoGen.Interface.EntitiesSlices.UserForm;
using RepoGen.Entities;
using FluentAssertions;

namespace RepoGenTest.Unit.EntitiesHandlers.Commands
{
    public class UpdateUserCommandHandlerTest
    {
        [Fact]
        public async Task Should_UpdateUser_IfExistent_NoReturn()
        {
            // Arrange
            var factory = FakeDbContextFactory.Create();
            var context = factory.CreateDbContext();

            var existentUser = await context.Users.AsNoTracking().FirstAsync();
            var noIncludedReport = await context.Permissions.AsNoTracking().FirstOrDefaultAsync(p => p.Name == "PermissionNotIncluded");
            existentUser.Name = "OtherName";

            var command = new UpdateUserCommand(
                existentUser.Id,
                existentUser.Name,
                existentUser.Login,
                string.Empty,
                existentUser.Admin,
                existentUser.Status,
                [noIncludedReport.Id]
            );
            var handler = new UpdateUserCommandHandler(context);

            // Act
            await handler.Handle(command, default);

            // Assert
            var userUpdated = await context.Users.AsNoTracking().Include(u => u.Permissions)
                .FirstOrDefaultAsync(u => u.Id == existentUser.Id);
            userUpdated.Should().NotBeNull();
            userUpdated.Name.Should().Be(existentUser.Name);
            userUpdated.Permissions.Should().NotBeNull();
            userUpdated.Permissions.Count.Should().Be(1);
            userUpdated.Permissions.First().Id.Should().Be(noIncludedReport.Id);
        }

        [Fact]
        public async Task Should_NotUpdateUser_BecauseNoExists_ThrowsException()
        {
            // Arrange
            var factory = FakeDbContextFactory.Create();
            var context = factory.CreateDbContext();

            var command = new UpdateUserCommand(
                Guid.NewGuid(),
                string.Empty,
                string.Empty,
                string.Empty,
                EnumStatus.Inactive,
                EnumStatus.Inactive,
                []
            );
            var handler = new UpdateUserCommandHandler(context);

            // Act
            Func<Task> act = async () => await handler.Handle(command, default);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("User not found");
        }
    }
}
