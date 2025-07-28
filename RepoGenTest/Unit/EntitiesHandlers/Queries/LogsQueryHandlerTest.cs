using RepoGen.Interface.EntitiesSlices.Logs;
using FluentAssertions;

namespace RepoGenTest.Unit.EntitiesHandlers.Queries
{
    public class LogsQueryHandlerTest
    {
        [Fact]
        internal async Task Should_GetLogs_ReturnsLogs()
        {
            // Arrange
            var fakeDbContextFactory = FakeDbContextFactory.Create();
            var fakeContext = fakeDbContextFactory.CreateDbContext();

            var command = new LogsQuery();
            var handler = new LogsQueryHandler(fakeContext);

            // Act
            var logs = await handler.Handle(command, default);

            // Assert
            logs.Should().NotBeNull();
            logs.Count.Should().BeGreaterThan(0);
        }
    }
}
