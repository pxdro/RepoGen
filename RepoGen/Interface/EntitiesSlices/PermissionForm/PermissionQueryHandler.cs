using MediatR;
using Microsoft.EntityFrameworkCore;
using RepoGen.Contexts;
using RepoGen.Entities;

namespace RepoGen.Interface.EntitiesSlices.PermissionForm
{
    // Query
    public record PermissionQuery(Guid Id) : IRequest<Permission>;

    // Handler
    public class PermissionQueryHandler(AppDbContext context) : IRequestHandler<PermissionQuery, Permission>
    {
        private readonly AppDbContext _context = context;

        public async Task<Permission> Handle(PermissionQuery request, CancellationToken cancellationToken)
        {
            return await _context.Permissions.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        }
    }
}
