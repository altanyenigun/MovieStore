using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieStoreApi.Common.Response;
using MovieStoreApi.Data;
using MovieStoreApi.DTOs;
using MovieStoreApi.Models;
using MovieStoreApi.Operation.Cqrs;

namespace Vk.Operation.Command;

public class ActorCommandHandler : 
    IRequestHandler<CreateActorCommand,ApiResponse<ActorResponse>>,
    IRequestHandler<UpdateActorCommand,ApiResponse>,
    IRequestHandler<DeleteActorCommand,ApiResponse>
    
{
    private readonly DataContext _dbContext;
    private readonly IMapper _mapper;

    public ActorCommandHandler(DataContext dbContext,IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    
    public async Task<ApiResponse<ActorResponse>> Handle(CreateActorCommand request, CancellationToken cancellationToken)
    {
        Actor mapped = _mapper.Map<Actor>(request.Model);
        var entity = await _dbContext.Actors.AddAsync(mapped,cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var response = _mapper.Map<ActorResponse>(entity.Entity);

        return new ApiResponse<ActorResponse>(response);
    }

    public async Task<ApiResponse> Handle(UpdateActorCommand request, CancellationToken cancellationToken)
    {
       var entity = await _dbContext.Actors.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
       if (entity == null)
       {
           return new ApiResponse("Actor not found!");
       }

       _mapper.Map(request.Model,entity);
       
       await _dbContext.SaveChangesAsync(cancellationToken);
       return new ApiResponse();
    }

    public async Task<ApiResponse> Handle(DeleteActorCommand request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Actors.Include(x=>x.Movies).FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (entity == null)
        {
            return new ApiResponse("Actor not found!");
        }
        if (entity.Movies is not null && entity.Movies.Any())
        {
            return new ApiResponse("You Cant delete this actor!, You should delete the movies it is linked to!.");
        }

        _dbContext.Actors.Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }
}