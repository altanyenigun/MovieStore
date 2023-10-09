using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieStoreApi.Common.Response;
using MovieStoreApi.Data;
using MovieStoreApi.DTOs;
using MovieStoreApi.Operation.Cqrs;


namespace MovieStoreApi.Operation.Query;

public class AccountQueryHandler :
    IRequestHandler<GetAllMovieQuery, ApiResponse<List<MovieResponse>>>
{
    private readonly DataContext _dbContext;
    private readonly IMapper _mapper;

    public AccountQueryHandler(DataContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }


    public async Task<ApiResponse<List<MovieResponse>>> Handle(GetAllMovieQuery request, CancellationToken cancellationToken)
    {
    
        var data = await _dbContext.Movies.Include(x => x.Actors).Include(x => x.Director).Include(x=>x.Genres).ToListAsync();
        var response = _mapper.Map<List<MovieResponse>>(data);

        List<MovieResponse> mapped = _mapper.Map<List<MovieResponse>>(response);
        return new ApiResponse<List<MovieResponse>>(mapped);
    }

}