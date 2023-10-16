using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MovieStoreApi.Common.Exceptions;
using MovieStoreApi.Common.Response;
using MovieStoreApi.Data;
using MovieStoreApi.DTOs;
using MovieStoreApi.Models;
using MovieStoreApi.Operation.Cqrs;
using MovieStoreApi.Operation.Validation;

namespace Vk.Operation.Command;

public class DirectorCommandHandler :
    IRequestHandler<CreateDirectorCommand, ApiResponse<DirectorResponse>>,
    IRequestHandler<UpdateDirectorCommand, ApiResponse>,
    IRequestHandler<DeleteDirectorCommand, ApiResponse>
{
    private readonly DataContext _dbContext;
    private readonly IMapper _mapper;

    public DirectorCommandHandler(DataContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }


    public async Task<ApiResponse<DirectorResponse>> Handle(CreateDirectorCommand request, CancellationToken cancellationToken)
    {
        CreateDirectorValidator validator = new CreateDirectorValidator();
        await validator.ValidateAndThrowAsync(request.Model, cancellationToken);

        Director mapped = _mapper.Map<Director>(request.Model);
        var entity = await _dbContext.Directors.AddAsync(mapped, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var response = _mapper.Map<DirectorResponse>(entity.Entity);

        return new ApiResponse<DirectorResponse>(response);
    }

    public async Task<ApiResponse> Handle(UpdateDirectorCommand request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Directors.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (entity == null)
        {
            throw CustomExceptions.DIRECTOR_NOT_FOUND;
        }

        UpdateDirectorValidator validator = new UpdateDirectorValidator();
        await validator.ValidateAndThrowAsync(request.Model, cancellationToken);

        _mapper.Map(request.Model, entity);

        await _dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }

    public async Task<ApiResponse> Handle(DeleteDirectorCommand request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Directors.Include(x => x.Movies).FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (entity == null)
        {
            throw CustomExceptions.DIRECTOR_NOT_FOUND;
        }
        if (entity.Movies is not null && entity.Movies.Any())
        {
            throw CustomExceptions.CANT_DELETE_DIRECTOR;
        }

        _dbContext.Directors.Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }
}