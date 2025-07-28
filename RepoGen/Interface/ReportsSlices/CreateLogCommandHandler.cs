using MediatR;
using RepoGen.Contexts;
using RepoGen.Entities;
using System.Security.Claims;

namespace RepoGen.Interface.ReportsSlices
{
    // Command
    public record CreateLogCommand(string? Descricao) : IRequest;

    // Handler
    public class CreateLogCommandHandler(AppDbContext context, IHttpContextAccessor httpContextAccessor) : IRequestHandler<CreateLogCommand>
    {
        private readonly AppDbContext _context = context;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task Handle(CreateLogCommand request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext?.User;

            var entity = new LogContent
            {
                User = user?.FindFirst(ClaimTypes.GivenName)?.Value,
                Report = request.Descricao,
                Data = DateTime.Now
            };

            _context.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}