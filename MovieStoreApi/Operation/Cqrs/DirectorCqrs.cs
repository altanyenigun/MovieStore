using MediatR;
using MovieStoreApi.Common.Response;
using MovieStoreApi.DTOs;

namespace MovieStoreApi.Operation.Cqrs;

public record CreateDirectorCommand(DirectorCreateRequest Model) : IRequest<ApiResponse<DirectorResponse>>;
public record UpdateDirectorCommand(DirectorUpdateRequest Model,int Id) : IRequest<ApiResponse>;
public record DeleteDirectorCommand(int Id) : IRequest<ApiResponse>;
public record GetAllDirectorQuery() : IRequest<ApiResponse<List<DirectorResponse>>>;
public record GetDirectorByIdQuery(int Id) : IRequest<ApiResponse<DirectorResponse>>;