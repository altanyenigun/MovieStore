using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieStoreApi.Common.Response;
using MovieStoreApi.Data;
using MovieStoreApi.DTOs;
using MovieStoreApi.Operation.Cqrs;


namespace MovieStoreApi.Operation.Query;

public class ActorQueryHandler :
    IRequestHandler<GetAllActorQuery, ApiResponse<List<ActorResponse>>>,
    IRequestHandler<GetActorByIdQuery, ApiResponse<ActorResponse>>

{
    private readonly DataContext _dbContext;
    private readonly IMapper _mapper;

    public ActorQueryHandler(DataContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }


    public async Task<ApiResponse<List<ActorResponse>>> Handle(GetAllActorQuery request, CancellationToken cancellationToken)
    {

        var data = await _dbContext.Actors.Include(x => x.Movies).ToListAsync();
        var response = _mapper.Map<List<ActorResponse>>(data);

        List<ActorResponse> mapped = _mapper.Map<List<ActorResponse>>(response);
        return new ApiResponse<List<ActorResponse>>(mapped);
    }

    public async Task<ApiResponse<ActorResponse>> Handle(GetActorByIdQuery request, CancellationToken cancellationToken)
    {
        var data = await _dbContext.Actors.Include(x => x.Movies).FirstOrDefaultAsync(x=>x.Id==request.Id);
        var response = _mapper.Map<ActorResponse>(data);

        ActorResponse mapped = _mapper.Map<ActorResponse>(response);
        return new ApiResponse<ActorResponse>(mapped);
    }

}