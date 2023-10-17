using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieStoreApi.Common.Exceptions;
using MovieStoreApi.Common.Response;
using MovieStoreApi.Data;
using MovieStoreApi.DTOs;
using MovieStoreApi.Models;
using MovieStoreApi.Operation.Cqrs;

namespace MovieStoreApi.Operation.Command;

public class CustomerCommandHandler :
    IRequestHandler<CustomerBuyMovieCommand, ApiResponse>,
    IRequestHandler<CustomerAddFavoriteGenreCommand, ApiResponse>

{
    private readonly DataContext _dbContext;
    private readonly IMapper _mapper;

    public CustomerCommandHandler(DataContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }


    public async Task<ApiResponse> Handle(CustomerBuyMovieCommand request, CancellationToken cancellationToken)
    {
        var movie = await _dbContext.Movies.Include(x => x.Actors).Include(x => x.Director).Include(x => x.Genres).Include(x => x.Customers).FirstOrDefaultAsync(x => x.Id == request.movieId);
        if (movie is null)
        {
            throw CustomExceptions.MOVIE_NOT_FOUND;
        }

        var customer = await _dbContext.Customers.Include(x => x.Movies).FirstOrDefaultAsync(x => x.Id == request.customerId);

        if (customer.Movies.Any(x => x.Id == movie.Id))
        {
            throw CustomExceptions.ALREADY_HAVE_THIS_MOVIE;
        }

        customer.Movies.Add(movie);
        var order = new Order
        {
            CustomerId = request.customerId,
            MovieId = request.movieId,
            Price = movie.Price,
            PurchaseDate = DateTime.Now

        };
        await _dbContext.Orders.AddAsync(order, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new ApiResponse();
    }

    public async Task<ApiResponse> Handle(CustomerAddFavoriteGenreCommand request, CancellationToken cancellationToken)
    {
        var genre = await _dbContext.Genres.FirstOrDefaultAsync(x => x.Id == request.genreId);
        if (genre is null)
        {
            throw CustomExceptions.GENRE_NOT_FOUND;
        }

        var customer = await _dbContext.Customers.Include(x => x.Genres).FirstOrDefaultAsync(x => x.Id == request.customerId);

        if (customer.Genres.Any(x => x.Id == genre.Id))
        {
            throw CustomExceptions.ALREADY_HAVE_THIS_GENRE;
        }
        
        customer.Genres.Add(genre);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new ApiResponse();
    }
}