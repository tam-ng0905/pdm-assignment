using Application.Core;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Domain;
using MediatR;
using Persistence;


//The is the logic to create book

namespace Application.Titles;
public class Create
{
    //Command does not return anything, unlike Query that used for List and Edit
    public class Command : IRequest<Result<Unit>>
    {
        public Title Title { get; set; }
        
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly DataContext _context;
        
        //Used to get the users
        private readonly IUserAccessor _userAccessor;

        public Handler(DataContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            
            //Find the user based of the Username of current user
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());
            
            
            //Set up the new object for linking table between users and books
            var owner = new BookOwner{
                User  = user,
                Title = request.Title,
                Owned = true,
            };
                
            
            request.Title.Owner.Add(owner);


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
            

            _context.Titles.Add(request.Title);
            var result = await _context.SaveChangesAsync() > 0;
            if (!result) return Result<Unit>.Failure("Failed to create book");
            return Result<Unit>.Success(Unit.Value);
        }
    }
}