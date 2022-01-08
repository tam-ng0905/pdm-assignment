using Domain;
using MediatR;
using Persistence;

//The is the logic to create author
namespace Application.Authors;
public class Create
{
    //Command does not return anything
    public class Command : IRequest
    {
        public Author Author { get; set; }
        
    }

    public class Handler : IRequestHandler<Command>
    {
        private readonly DataContext _context;

        public Handler(DataContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        { 
            _context.Authors.Add(request.Author);
            await _context.SaveChangesAsync();
            return Unit.Value;
        }
    }
}