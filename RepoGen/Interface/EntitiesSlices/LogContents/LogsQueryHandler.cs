using MediatR;
using Microsoft.EntityFrameworkCore;
using RepoGen.Contexts;
using RepoGen.Entities;

namespace RepoGen.Interface.EntitiesSlices.Logs
{
    // Query
    public record LogsQuery : IRequest<List<LogContent>>;

    // Handler
    public class LogsQueryHandler(AppDbContext context) : IRequestHandler<LogsQuery, List<LogContent>>
    {
        private readonly AppDbContext _context = context;

        public async Task<List<LogContent>> Handle(LogsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Logs.AsNoTracking().ToListAsync(cancellationToken);
        }
    }
}
