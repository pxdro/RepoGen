using System.Security.Claims;
using RepoGen.Interface.EntitiesSlices.Auth;
using Moq;
using FluentAssertions;
using RepoGen.Entities;

namespace RepoGenTest.Unit.EntitiesHandlers.Commands
{
    public class AuthCommandHandlerTest
    {
        [Fact]
        internal async Task Should_AuthUser_ReturnsPrincipal()
        {
            // Arrange
            var fakeDbContextFactory = FakeDbContextFactory.Create();
            var fakeContext = fakeDbContextFactory.CreateDbContext();

            var plainPassword = "123456";
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(plainPassword);

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = "FakeUser123",
                Login = "fakeuser123",
                HashedPassword = hashedPassword,
                Status = EnumStatus.Active,
                Admin = EnumStatus.Active,
                Permissions =
                [
                    new() { Name = "PermissionIncluded" }
                ]
            };

            fakeContext.Users.Add(user);
            await fakeContext.SaveChangesAsync();

            var command = new AuthCommand(user.Login, plainPassword);
            var handler = new AuthCommandHandler(fakeContext);

            // Act
            var principal = await handler.Handle(command, default);

            // Assert
            var identity = (ClaimsIdentity?)principal.Identity;
            var roles = identity?.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();

            identity.Should().NotBeNull();
            identity.IsAuthenticated.Should().BeTrue();
            identity.Name.Should().Be(user.Name);
            identity.FindFirst(ClaimTypes.GivenName)!.Value.Should().Be(user.Login);
            identity.FindFirst(ClaimTypes.NameIdentifier)!.Value.Should().Be(user.Id.ToString());
            roles.Should().NotBeNull();
            roles.Should().Contain("Admin");
            roles.Should().Contain("PermissionIncluded");
        }

        [Fact]
        internal async Task Should_NotAuthUser_UserNotFound_ThrowsException()
        {
            // Arrange
            var fakeDbContextFactory = FakeDbContextFactory.Create();
            var fakeContext = fakeDbContextFactory.CreateDbContext();
            
            var command = new AuthCommand("NoExistentUser", "AnyPassword");
            var handler = new AuthCommandHandler(fakeContext);

            // Act
            Func<Task> act = async () => await handler.Handle(command, default);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("User not found");
        }

        [Fact]
        internal async Task Should_NotAuthUser_InactiveUser_ThrowsException()
        {
            // Arrange
            var fakeDbContextFactory = FakeDbContextFactory.Create();
            var fakeContext = fakeDbContextFactory.CreateDbContext();

            var clearPassword = "clearpassword";
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(clearPassword);
            var inactiveUser = new User
            {
                Name = "InactiveUser",
                Login = "inactiveuser",
                HashedPassword = hashedPassword,
                Status = EnumStatus.Inactive,
                Admin = EnumStatus.Inactive
            };
            await fakeContext.AddAsync(inactiveUser);
            await fakeContext.SaveChangesAsync();

            var command = new AuthCommand("inactiveuser", clearPassword);
            var handler = new AuthCommandHandler(fakeContext);

            // Act
            Func<Task> act = async () => await handler.Handle(command, default);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("User inactive");
        }

        [Fact]
        internal async Task Should_NotAuthUser_WrongPassword_ThrowsException()
        {
            // Arrange
            var fakeDbContextFactory = FakeDbContextFactory.Create();
            var fakeContext = fakeDbContextFactory.CreateDbContext();

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword("clearpassword");
            var user = new User
            {
                Name = "FakeUser123",
                Login = "fakeuser123",
                HashedPassword = hashedPassword,
                Status = EnumStatus.Active,
                Admin = EnumStatus.Inactive
            };

            fakeContext.Users.Add(user);
            await fakeContext.SaveChangesAsync();

            var command = new AuthCommand("fakeuser123", "WrongPassword");
            var handler = new AuthCommandHandler(fakeContext);

            // Act
            Func<Task> act = async () => await handler.Handle(command, default);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Wrong password");
        }
    }
}