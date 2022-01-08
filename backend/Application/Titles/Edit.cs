using Application.Core;
using RedLockNet.SERedis;
using AutoMapper;
using Domain;
using MediatR;
using Persistence;

//The is the logic to return book's detail
namespace Application.Titles;
public class Edit
{
    public class Command : IRequest<Result<Unit>>
    {
        public Title Title { get; set; }
        
    }
    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly RedLockFactory _redLockFactory;

        public Handler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {

            //Get the title from the db based on Id
            var title = await _context.Titles.FindAsync(request.Title.Id);
            
            if (title == null) return null;
            
            _mapper.Map(request.Title, title);
            
            //Checking whether the author already existed in the database or not
            var authorList = _context.Authors.Where(author => author.FirstName == request.Title.Author.FirstName)
                .Where(author => author.LastName == request.Title.Author.LastName)
                .ToList();
            Author author;
            
            //If the author already exists, we use the same data from the database
            //Else, we create a new author object
            if (authorList.Count() > 0)
            {
                author = authorList[0];
                request.Title.Author = author;
            }
            
            var result = await _context.SaveChangesAsync() > 0;
            
            
            if (!result) return Result<Unit>.Failure("Failed to update book");

            return Result<Unit>.Success(Unit.Value);
            

        }
    }
}