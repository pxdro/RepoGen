using Microsoft.EntityFrameworkCore;
using RepoGen.Entities;
using RepoGen.Interface.EntitiesSlices.UserForm;
using FluentAssertions;

namespace RepoGenTest.Unit.EntitiesHandlers.Commands
{
    public class CreateUserCommandHandlerTest
    {
        [Fact]
        public async Task Should_CreateUser_IfNotExistent_NoReturn()
        {
            // Arrange
            var factory = FakeDbContextFactory.Create();
            var context = factory.CreateDbContext();

            var firstPermission = await context.Permissions.AsNoTracking().FirstAsync();
            var newUser = new User
            {
                Name = "NewUser",
                Login = "newuser",
                HashedPassword = "clearpassword",
                Admin = EnumStatus.Inactive,
                Status = EnumStatus.Active,
            };

            var command = new CreateUserCommand(
                newUser.Name,
                newUser.Login,
                newUser.HashedPassword,
                newUser.Admin,
                newUser.Status,
                [firstPermission.Id]
            );
            var handler = new CreateUserCommandHandler(context);

            // Act
            await handler.Handle(command, default);

            // Assert
            var userCreated = await context.Users.AsNoTracking().Include(u => u.Permissions)
                .FirstOrDefaultAsync(u => u.Name == newUser.Name);
            userCreated.Should().NotBeNull();
            userCreated.Login.Should().Be(newUser.Login);
            userCreated.Admin.Should().Be(newUser.Admin);
            userCreated.Status.Should().Be(newUser.Status);
            userCreated.Permissions.Should().NotBeNull();
            userCreated.Permissions.Count.Should().Be(1);
            userCreated.Permissions.First().Id.Should().Be(firstPermission.Id);
        }

        [Fact]
        public async Task Should_NotCreateUser_BecauseExists_ThrowsException()
        {
            // Arrange
            var factory = FakeDbContextFactory.Create();
            var context = factory.CreateDbContext();

            var firstUser = await context.Users.AsNoTracking().FirstAsync();

            var command = new CreateUserCommand(
                firstUser.Name,
                firstUser.Login,
                string.Empty,
                firstUser.Admin,
                firstUser.Status,
                []
            );
            var handler = new CreateUserCommandHandler(context);

            // Act
            Func<Task> act = async () => await handler.Handle(command, default);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("User with this login already exists");
        }
    }
}
