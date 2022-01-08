using Application.Core;
using Application.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using Microsoft.Extensions.Logging;
using Persistence;
using MediatR;


//The is the logic to return book's detail


namespace Application.Titles;
public class Search
{
    public class Query : IRequest<Result<PagedList<TitleDto>>>
    {
        public string query { get; set; }
        public int price { set; get; }
        
        public PagingParams Params { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<PagedList<TitleDto>>>
    {
        private readonly DataContext _context;
        private readonly ILogger<List> _logger;
        private readonly IUserAccessor _userAccessor;
        private readonly IMapper _mapper;

        public Handler(DataContext context, ILogger<List> logger, IUserAccessor userAccessor, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _userAccessor = userAccessor;
            _mapper = mapper;
        }
        public async Task<Result<PagedList<TitleDto>>> Handle(Query request, CancellationToken cancellationToken)
        {


            var queryToLower = request.query.ToLower();
            
            //If there is no price in the request, search for the name only
            if (request.price <= 0)
            {
                var query = _context.Titles
                    .ProjectTo<TitleDto>(_mapper.ConfigurationProvider,
                        new {currentUsername = _userAccessor.GetUsername()}).AsNoTracking()
                    .Where(book => book.Name.ToLower().Contains(queryToLower))
                    .AsQueryable();
                query = query.Where(x => x.OwnerName == _userAccessor.GetUsername());
                
                return Result<PagedList<TitleDto>>.Success(
                    await PagedList<TitleDto>.CreateAsync(query, request.Params.PageNumber, request.Params.PageSize));
            }
            else
            {
                //If there is price, search for the price as well
                var query = _context.Titles
                    .ProjectTo<TitleDto>(_mapper.ConfigurationProvider,
                        new
                        {
                            currentUsername = _userAccessor.GetUsername(),
                        })
                    .AsNoTracking()
                    .Where(book => book.Name.ToLower().Contains(queryToLower))
                    .Where(book => book.Price < request.price);
                query = query.Where(x => x.OwnerName == _userAccessor.GetUsername());
                
                return Result<PagedList<TitleDto>>.Success(
                    await PagedList<TitleDto>.CreateAsync(query, request.Params.PageNumber, request.Params.PageSize));
            }
        }
        
    }
}