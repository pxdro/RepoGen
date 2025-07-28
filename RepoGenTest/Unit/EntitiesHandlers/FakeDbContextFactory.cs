using Microsoft.EntityFrameworkCore;
using RepoGen.Contexts;
using RepoGen.Entities;
using Moq;

namespace RepoGenTest.Unit.EntitiesHandlers
{
    internal class FakeDbContextFactory
    {
        public static IDbContextFactory<AppDbContext> Create()
        {
            var dbName = $"TestDb_{Guid.NewGuid()}";

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            var context = new AppDbContext(options);

            Seed(context);

            var factoryMock = new Mock<IDbContextFactory<AppDbContext>>();
            factoryMock
                .Setup(f => f.CreateDbContext())
                .Returns(context);

            return factoryMock.Object;
        }

        private static void Seed(AppDbContext context)
        {
            var includedPermission = new Permission { Name = "PermissionIncluded" };
            var notIncludedPermission = new Permission { Name = "PermissionNotIncluded" };

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword("plainpassword");
            var user = new User
            {
                Name = "ActiveUser",
                Login = "activeuser",
                HashedPassword = hashedPassword,
                Admin = EnumStatus.Active,
                Status = EnumStatus.Active,
                Permissions = [includedPermission]
            };

            var logs = new List<LogContent>
            {
                new()
                {
                    Data = DateTime.UtcNow.AddHours(-1),
                    User = user.Login,
                    Report = notIncludedPermission.Name,
                },
                new()
                {
                    Data = DateTime.UtcNow,
                    User = user.Login,
                    Report = includedPermission.Name,
                }
            };

            context.Permissions.AddRange([includedPermission, notIncludedPermission]);
            context.Users.Add(user);
            context.Logs.AddRange(logs);

            context.SaveChanges();
        }
    }
}
