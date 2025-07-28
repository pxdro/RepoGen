using MediatR;
using Microsoft.EntityFrameworkCore;
using RepoGen.Contexts;
using RepoGen.Entities;

namespace RepoGen.Interface.EntitiesSlices.UserForm
{
    // Command
    public record UpdateUserCommand(Guid Id, string Name, string Login, string? Password, EnumStatus Admin, EnumStatus Status, ICollection<Guid> PermissionsIds) : IRequest;

    // Handler
    public class UpdateUserCommandHandler(AppDbContext context) : IRequestHandler<UpdateUserCommand>
    {
        private readonly AppDbContext _context = context;

        public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.Include(x => x.Permissions)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
                ?? throw new Exception("User not found");

            user.Name = request.Name;
            user.Login = request.Login;
            if (!string.IsNullOrEmpty(request.Password))
                user.HashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
            user.Admin = request.Admin;
            user.Status = request.Status;
            user.Permissions = await _context.Permissions
                .Where(p => request.PermissionsIds.Contains(p.Id))
                .ToListAsync(cancellationToken);
            _context.Users.Update(user);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
