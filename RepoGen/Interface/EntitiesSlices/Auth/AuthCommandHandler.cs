using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using RepoGen.Contexts;
using RepoGen.Entities;
using System.Security.Claims;

namespace RepoGen.Interface.EntitiesSlices.Auth
{
    // Command
    public record AuthCommand(string Login, string Password) : IRequest<ClaimsPrincipal>;

    // Handler
    public class AuthCommandHandler(AppDbContext context) : IRequestHandler<AuthCommand, ClaimsPrincipal>
    {
        private readonly AppDbContext _context = context;

        public async Task<ClaimsPrincipal> Handle(AuthCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .AsNoTracking()
                .Include(x => x.Permissions)
                .FirstOrDefaultAsync(x => x.Login == request.Login, cancellationToken)
                ?? throw new Exception("User not found");

            if (user.Status == EnumStatus.Inactive)
                throw new Exception("User inactive");

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.HashedPassword))
                throw new Exception("Wrong password");

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.Name),
                new(ClaimTypes.GivenName, user.Login),
                new(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            if (user.Admin == EnumStatus.Active)
                claims.Add(new(ClaimTypes.Role, "Admin"));

            foreach (var permission in user.Permissions)
                claims.Add(new(ClaimTypes.Role, permission.Name));

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            return new ClaimsPrincipal(identity);
        }
    }
}