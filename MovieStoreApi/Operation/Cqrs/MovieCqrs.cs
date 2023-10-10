using MediatR;
using MovieStoreApi.Common.Response;
using MovieStoreApi.DTOs;

namespace MovieStoreApi.Operation.Cqrs;

public record CreateMovieCommand(MovieCreateRequest Model) : IRequest<ApiResponse<MovieResponse>>;
public record UpdateMovieCommand(MovieUpdateRequest Model, int Id) : IRequest<ApiResponse>;
public record DeleteMovieCommand(int Id) : IRequest<ApiResponse>;
public record GetAllMovieQuery() : IRequest<ApiResponse<List<MovieResponse>>>;
public record GetMovieByIdQuery(int Id) : IRequest<ApiResponse<MovieResponse>>;