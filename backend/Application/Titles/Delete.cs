using Application.Core;
using MediatR;
using Persistence;

//The is the logic to delete book

namespace Application.Titles;
public class Delete
{
    public class Command : IRequest<Result<Unit>>
    {
        public Guid Id { get; set; }
        
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly DataContext _context;

        public Handler(DataContext context)
        {
            _context = context;
        }
        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var title = await _context.Titles.FindAsync(request.Id);

            _context.Remove(title);

            await _context.SaveChangesAsync();
            
            return Result<Unit>.Success(Unit.Value);
        }
    }
}