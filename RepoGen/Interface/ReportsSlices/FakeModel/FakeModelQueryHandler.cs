using Dapper;
using MediatR;
using Microsoft.Data.SqlClient;

namespace RepoGen.Interface.ReportsSlices.FakeModel
{
    // Dto
    public record struct FakeModelDto(
        string? Property
    );

    // Query
    public record FakeModelQuery : IRequest<IEnumerable<FakeModelDto>>;

    // Handler
    public class FakeModelQueryHandler(IConfiguration configuration) : IRequestHandler<FakeModelQuery, IEnumerable<FakeModelDto>>
    {
        public async Task<IEnumerable<FakeModelDto>> Handle(FakeModelQuery request, CancellationToken cancellationToken)
        {
            /*
            using var connection = new SqlConnection(configuration.GetConnectionString("OtherDbConnection"));
            const string sql =
                "sql";
            var result = await connection.QueryAsync<FakeModelDto>(sql);
            */

            var result = new List<FakeModelDto>
            {
                new("Prop1"),
                new("Prop2"),
                new("Prop3"),
                new("Prop4"),
                new("Prop5"),
            };

            return result;
        }
    }
}
