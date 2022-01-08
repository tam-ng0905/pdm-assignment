using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence;
using MediatR;

//The is the logic to return all authors
namespace Application.Authors;

public class List
{
    public class Query : IRequest<List<Author>> {}

    public class Handler : IRequestHandler<Query, List<Author>>
    {
        private readonly DataContext _context;
        private readonly ILogger<List> _logger;

        public Handler(DataContext context, ILogger<List> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<List<Author>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _context.Authors.ToListAsync(cancellationToken);
        }
        
    }
}