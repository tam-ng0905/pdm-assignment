using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;


//The is the logic to return book's detail

namespace Application.Titles;
public class Details
{
    public class Query : IRequest<Result<TitleDto>>
    {
        public Guid Id { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<TitleDto>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public Handler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Result<TitleDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var title = await _context.Titles
                .ProjectTo<TitleDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(x => x.Id == request.Id);
            return Result<TitleDto>.Success(title);
        }
    }
        
}