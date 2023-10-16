using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieStoreApi.Common.Exceptions;
using MovieStoreApi.Common.Response;
using MovieStoreApi.Data;
using MovieStoreApi.DTOs;
using MovieStoreApi.Models;
using MovieStoreApi.Operation.Cqrs;
using MovieStoreApi.Operation.Validation;

namespace Vk.Operation.Command;

public class MovieCommandHandler :
    IRequestHandler<CreateMovieCommand, ApiResponse<MovieResponse>>,
    IRequestHandler<UpdateMovieCommand, ApiResponse>,
    IRequestHandler<DeleteMovieCommand, ApiResponse>
{
    private readonly DataContext _dbContext;
    private readonly IMapper _mapper;

    public MovieCommandHandler(DataContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }


    public async Task<ApiResponse<MovieResponse>> Handle(CreateMovieCommand request, CancellationToken cancellationToken)
    {
        CreateMovieValidator validator = new CreateMovieValidator(_dbContext);
        await validator.ValidateAndThrowAsync(request.Model, cancellationToken);

        Movie mapped = _mapper.Map<Movie>(request.Model);

        List<Genre> genres = _dbContext.Genres.Where(g => request.Model.GenreIds.Contains(g.Id)).ToList();
        Director director = _dbContext.Directors.FirstOrDefault(d => d.Id == request.Model.DirectorId);
        List<Actor> actors = _dbContext.Actors.Where(a => request.Model.ActorIds.Contains(a.Id)).ToList();

        mapped.Genres = genres;
        mapped.Director = director;
        mapped.Actors = actors;

        var entity = await _dbContext.Movies.AddAsync(mapped, cancellationToken);
        await _dbContext.SaveChangesAsync();

        var response = _mapper.Map<MovieResponse>(entity.Entity);

        return new ApiResponse<MovieResponse>(response);
    }

    public async Task<ApiResponse> Handle(UpdateMovieCommand request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Movies.Include(x => x.Actors).Include(x => x.Director).Include(x => x.Genres).FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (entity is null)
        {
            throw CustomExceptions.MOVIE_NOT_FOUND;
        }

        var movieNameIsExist = await _dbContext.Movies.FirstOrDefaultAsync(x=>x.Name == request.Model.Name && x.Id != entity.Id);
        if(movieNameIsExist is not null)
        {
            throw CustomExceptions.MOVIE_NAME_ALREADY_IN_USE;
        }

        UpdateMovieValidator validator = new UpdateMovieValidator(_dbContext);
        await validator.ValidateAndThrowAsync(request.Model, cancellationToken);

        _mapper.Map(request.Model, entity);

        List<Genre> genres = _dbContext.Genres.Where(g => request.Model.GenreIds.Contains(g.Id)).ToList();
        Director director = _dbContext.Directors.FirstOrDefault(d => d.Id == request.Model.DirectorId);
        List<Actor> actors = _dbContext.Actors.Where(a => request.Model.ActorIds.Contains(a.Id)).ToList();

        entity.Genres = genres;
        entity.Director = director;
        entity.Actors = actors;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }

    public async Task<ApiResponse> Handle(DeleteMovieCommand request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Movies.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (entity == null)
        {
            throw CustomExceptions.MOVIE_NOT_FOUND;
        }
        entity.isActive = false;
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }

}