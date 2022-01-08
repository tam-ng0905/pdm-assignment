using Application.Core;
using Microsoft.Extensions.Logging;
using Persistence;
using MediatR;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.Extensions.Caching.Distributed;

//The is the logic to return the seed books

namespace Application.Titles;
public class Seed
{
    public class Query : IRequest<Result<PagedList<TitleDto>>>
    {
        public PagingParams Params { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<PagedList<TitleDto>>>
    {
        private readonly DataContext _context;
        private readonly ILogger<List> _logger;
        private readonly IUserAccessor _userAccessor;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;

        public Handler(DataContext context, ILogger<List> logger, IUserAccessor userAccessor, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _userAccessor = userAccessor;
            _mapper = mapper;
        }
        public async Task<Result<PagedList<TitleDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            
            var query = _context.Titles
                .ProjectTo<TitleDto>(_mapper.ConfigurationProvider,
                    new { currentUsername = _userAccessor.GetUsername()})
                .Take(50)
                .AsQueryable();
            query = query.Where(x => x.OwnerName == _userAccessor.GetUsername());
            
            //Create the pagedList
            return Result<PagedList<TitleDto>>.Success(
                await PagedList<TitleDto>.CreateAsync(query, request.Params.PageNumber, request.Params.PageSize)
            );
        }
        
    }
}