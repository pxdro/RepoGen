using Microsoft.EntityFrameworkCore;
using RepoGen.Contexts;
using MediatR;

namespace RepoGen.Interface.EntitiesSlices.Permissions
{
    // Command
    public record DeletePermissionCommand(Guid Id) : IRequest;

    // Handler
    public class DeletePermissionCommandHandler(AppDbContext context) : IRequestHandler<DeletePermissionCommand>
    {
        private readonly AppDbContext _context = context;

        public async Task Handle(DeletePermissionCommand request, CancellationToken cancellationToken)
        {
            var permission =
                await _context.Permissions.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
                ?? throw new Exception("Permission not found");
            _context.Permissions.Remove(permission);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}