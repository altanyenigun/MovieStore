using MediatR;
using MovieStoreApi.Common.Response;
using MovieStoreApi.DTOs;

namespace MovieStoreApi.Operation.Cqrs;

public record CreateActorCommand(ActorCreateRequest Model) : IRequest<ApiResponse<ActorResponse>>;
public record UpdateActorCommand(ActorUpdateRequest Model,int Id) : IRequest<ApiResponse>;
public record DeleteActorCommand(int Id) : IRequest<ApiResponse>;
public record GetAllActorQuery() : IRequest<ApiResponse<List<ActorResponse>>>;
public record GetActorByIdQuery(int Id) : IRequest<ApiResponse<ActorResponse>>;