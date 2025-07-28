using Microsoft.EntityFrameworkCore;
using RepoGen.Contexts;
using MediatR;

namespace RepoGen.Interface.EntitiesSlices.Users
{
    // Command
    public record DeleteUserCommand(Guid Id) : IRequest;

    // Handler
    public class DeleteUserCommandHandler(AppDbContext context) : IRequestHandler<DeleteUserCommand>
    {
        private readonly AppDbContext _context = context;

        public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user =
                await _context.Users.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
                ?? throw new Exception("User not found");
            _context.Users.Remove(user);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}