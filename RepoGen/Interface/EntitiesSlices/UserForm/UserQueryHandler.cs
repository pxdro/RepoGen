using MediatR;
using Microsoft.EntityFrameworkCore;
using RepoGen.Contexts;
using RepoGen.Entities;

namespace RepoGen.Interface.EntitiesSlices.UserForm
{
    // Query
    public record UserQuery(Guid Id) : IRequest<User?>;

    // Handler
    public class UserQueryHandler(AppDbContext context) : IRequestHandler<UserQuery, User?>
    {
        private readonly AppDbContext _context = context;

        public async Task<User?> Handle(UserQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.AsNoTracking().Include(x => x.Permissions)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (user != null)
                user.HashedPassword = string.Empty;
            return user;
        }
    }
}
