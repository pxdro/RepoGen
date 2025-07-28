using MediatR;
using Microsoft.EntityFrameworkCore;
using RepoGen.Contexts;
using RepoGen.Entities;

namespace RepoGen.Interface.EntitiesSlices.Users
{
    // Query
    public record UsersQuery : IRequest<IEnumerable<User>>;

    // Handler
    public class UsersQueryHandler(AppDbContext context) : IRequestHandler<UsersQuery, IEnumerable<User>>
    {
        private readonly AppDbContext _context = context;

        public async Task<IEnumerable<User>> Handle(UsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _context.Users
                .AsNoTracking()
                .Include(x => x.Permissions)
                .ToListAsync(cancellationToken);
            foreach (var user in users)
                user.HashedPassword = string.Empty;
            return users;
        }
    }
}