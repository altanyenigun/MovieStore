using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieStoreApi.Common.Response;
using MovieStoreApi.Data;
using MovieStoreApi.DTOs;
using MovieStoreApi.Models;
using MovieStoreApi.Operation.Cqrs;

namespace Vk.Operation.Command;

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
        var movie = await _dbContext.Movies.Include(x=>x.Actors).Include(x=>x.Director).Include(x=>x.Genres).Include(x=>x.Customers).FirstOrDefaultAsync(x => x.Id == request.movieId);
        if (movie is null)
        {
            return new ApiResponse("Movie not found!");
        }
        var customer = await _dbContext.Customers.Include(x=>x.Movies).FirstOrDefaultAsync(x => x.Id == request.customerId);
        customer.Movies.Add(movie);
        await _dbContext.SaveChangesAsync(cancellationToken);
    
        return new ApiResponse();
    }

    public async Task<ApiResponse> Handle(CustomerAddFavoriteGenreCommand request, CancellationToken cancellationToken)
    {
        var genre = await _dbContext.Genres.FirstOrDefaultAsync(x => x.Id == request.genreId);
        if (genre is null)
        {
            return new ApiResponse("Genre not found!");
        }
        var customer = await _dbContext.Customers.Include(x=>x.Genres).FirstOrDefaultAsync(x => x.Id == request.customerId);
        customer.Genres.Add(genre);
        await _dbContext.SaveChangesAsync(cancellationToken);
    
        return new ApiResponse();
    }
}