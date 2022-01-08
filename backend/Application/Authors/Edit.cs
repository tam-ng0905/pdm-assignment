using AutoMapper;
using Domain;
using MediatR;
using Persistence;

//The is the logic to update author
namespace Application.Authors;
public class Edit
{
    public class Command : IRequest
    {
        public Author Author { get; set; }
        
    }
    public class Handler : IRequestHandler<Command>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public Handler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            //Get the activity from the db based on Id
            var note = await _context.Authors.FindAsync(request.Author.Id);

            _mapper.Map(request.Author, note);
            
            await _context.SaveChangesAsync();
            
            return Unit.Value;

        }
    }
}