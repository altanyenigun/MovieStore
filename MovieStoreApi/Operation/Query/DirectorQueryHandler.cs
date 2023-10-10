using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieStoreApi.Common.Response;
using MovieStoreApi.Data;
using MovieStoreApi.DTOs;
using MovieStoreApi.Operation.Cqrs;


namespace MovieStoreApi.Operation.Query;

public class DirectorQueryHandler :
    IRequestHandler<GetAllDirectorQuery, ApiResponse<List<DirectorResponse>>>,
    IRequestHandler<GetDirectorByIdQuery, ApiResponse<DirectorResponse>>

{
    private readonly DataContext _dbContext;
    private readonly IMapper _mapper;

    public DirectorQueryHandler(DataContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }


    public async Task<ApiResponse<List<DirectorResponse>>> Handle(GetAllDirectorQuery request, CancellationToken cancellationToken)
    {
        var data = await _dbContext.Directors.Include(x => x.Movies).ToListAsync();
        var response = _mapper.Map<List<DirectorResponse>>(data);

        List<DirectorResponse> mapped = _mapper.Map<List<DirectorResponse>>(response);
        return new ApiResponse<List<DirectorResponse>>(mapped);
    }

    public async Task<ApiResponse<DirectorResponse>> Handle(GetDirectorByIdQuery request, CancellationToken cancellationToken)
    {
        var data = await _dbContext.Directors.Include(x => x.Movies).FirstOrDefaultAsync(x=>x.Id==request.Id);
        var response = _mapper.Map<DirectorResponse>(data);

        DirectorResponse mapped = _mapper.Map<DirectorResponse>(response);
        return new ApiResponse<DirectorResponse>(mapped);
    }

}