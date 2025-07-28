using MediatR;
using Microsoft.EntityFrameworkCore;
using RepoGen.Contexts;
using RepoGen.Entities;

namespace RepoGen.Interface.EntitiesSlices.PermissionForm
{
    // Command
    public record CreatePermissionCommand(string Name) : IRequest;

    // Handler
    public class CreatePermissionCommandHandler(AppDbContext context) : IRequestHandler<CreatePermissionCommand>
    {
        private readonly AppDbContext _context = context;

        public async Task Handle(CreatePermissionCommand request, CancellationToken cancellationToken)
        {
            var permission =
                await _context.Permissions.AsNoTracking().FirstOrDefaultAsync(x => x.Name == request.Name, cancellationToken);
            if (permission != null)
                throw new Exception("Permission with this name already exists");
            permission = new Permission { Name = request.Name };
            _context.Permissions.Add(permission);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
