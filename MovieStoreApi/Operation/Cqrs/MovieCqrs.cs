using MediatR;
using MovieStoreApi.Common.Response;
using MovieStoreApi.DTOs;

namespace MovieStoreApi.Operation.Cqrs;

public record GetAllMovieQuery() : IRequest<ApiResponse<List<MovieResponse>>>;
