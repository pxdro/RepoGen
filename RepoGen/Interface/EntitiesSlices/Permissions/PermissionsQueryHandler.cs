using MediatR;
using Microsoft.EntityFrameworkCore;
using RepoGen.Contexts;
using RepoGen.Entities;

namespace RepoGen.Interface.EntitiesSlices.Permissions
{
    // Query
    public record PermissionsQuery : IRequest<List<Permission>>;

    // Handler
    public class PermissionsQueryHandler(AppDbContext context) : IRequestHandler<PermissionsQuery, List<Permission>>
    {
        private readonly AppDbContext _context = context;

        public async Task<List<Permission>> Handle(PermissionsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Permissions.AsNoTracking()
                .ToListAsync(cancellationToken);
        }
    }
}