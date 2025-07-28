using MediatR;
using Microsoft.EntityFrameworkCore;
using RepoGen.Contexts;

namespace RepoGen.Interface.EntitiesSlices.PermissionForm
{
    // Command
    public record UpdatePermissionCommand(Guid Id, string Name) : IRequest;

    // Handler
    public class UpdatePermissionCommandHandler(AppDbContext context) : IRequestHandler<UpdatePermissionCommand>
    {
        private readonly AppDbContext _context = context;

        public async Task Handle(UpdatePermissionCommand request, CancellationToken cancellationToken)
        {
            var permission =
                await _context.Permissions.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
                ?? throw new Exception("Permission not found");
            permission.Name = request.Name;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
