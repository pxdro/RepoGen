using MediatR;
using Microsoft.EntityFrameworkCore;
using RepoGen.Contexts;
using RepoGen.Entities;

namespace RepoGen.Interface.EntitiesSlices.UserForm
{
    // Command
    public record CreateUserCommand(string Name, string Login, string Password, EnumStatus Admin, EnumStatus Status, ICollection<Guid> PermissionsIds) : IRequest;

    // Handler
    public class CreateUserCommandHandler(AppDbContext context) : IRequestHandler<CreateUserCommand>
    {
        private readonly AppDbContext _context = context;

        public async Task Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user =
                await _context.Users.FirstOrDefaultAsync(x => x.Login == request.Login, cancellationToken);
            if (user != null)
                throw new Exception("User with this login already exists");

            user = new User
            {
                Name = request.Name,
                Login = request.Login,
                HashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Admin = request.Admin,
                Status = request.Status,
                Permissions = await _context.Permissions
                    .Where(p => request.PermissionsIds.Contains(p.Id))
                    .ToListAsync(cancellationToken),
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}