using MediatR;
using MovieStoreApi.Common.Response;
using MovieStoreApi.DTOs;

namespace MovieStoreApi.Operation.Cqrs;

public record CustomerBuyMovieCommand(int customerId, int movieId) : IRequest<ApiResponse>;
public record CustomerAddFavoriteGenreCommand(int customerId, int genreId) : IRequest<ApiResponse>;