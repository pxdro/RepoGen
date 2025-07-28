using RepoGen.Interface.ReportsSlices.FakeModel;
using Microsoft.Extensions.Configuration;
using FluentAssertions;

namespace RepoGenTest.Integration
{
    public class ReportsHandlerTests
    {
        private readonly IConfiguration _configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();

        [Fact(Skip = "OtherDbConnection Not implemented")]
        public async Task Should_ReturnAcessoData()
        {
            // Arrange
            var handler = new FakeModelQueryHandler(_configuration);

            // Act
            var result = await handler.Handle(new FakeModelQuery(), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
        }

        /*
         * More reports should be tested here
         */
    }
}
