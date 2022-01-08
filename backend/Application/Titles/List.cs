using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence;
using MediatR;

//The is the logic to return all books

namespace Application.Titles;
public class List
{
    public class Query : IRequest<Result<PagedList<TitleDto>>>
    {
        public PagingParams Params { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<PagedList<TitleDto>>>
    {
        public IMapper Mapper { get; }
        private readonly DataContext _context;
        private readonly ILogger<List> _logger;
        private readonly IMapper _mapper;
        private readonly IUserAccessor _userAccessor;

        public Handler(DataContext context, ILogger<List> logger, IMapper mapper, IUserAccessor userAccessor)
        {
            _mapper = mapper;
            _userAccessor = userAccessor;
            _context = context;
            _logger = logger;
        }
        public async Task<Result<PagedList<TitleDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            //defer the execution until we actually create a pagedList
            var query = _context.Titles
                .ProjectTo<TitleDto>(_mapper.ConfigurationProvider,
                    new { currentUsername = _userAccessor.GetUsername() })
                .OrderBy(book => book.Name)
                .AsQueryable();

            
            //Create the pagedList
            return Result<PagedList<TitleDto>>.Success(
                await PagedList<TitleDto>.CreateAsync(query, request.Params.PageNumber, request.Params.PageSize)
                );
        }
        
    }
}