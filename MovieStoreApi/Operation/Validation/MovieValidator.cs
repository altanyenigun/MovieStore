using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MovieStoreApi.Data;
using MovieStoreApi.DTOs;

namespace MovieStoreApi.Operation.Validation;

public class CreateMovieValidator : AbstractValidator<MovieCreateRequest>
{

    public CreateMovieValidator(DataContext dbContext)
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50);

        RuleFor(x => x.Year)
           .GreaterThan(1900).WithMessage("Year must be greater than 1900.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0.");

        RuleFor(x => x.GenreIds)
            .NotEmpty().WithMessage("You must specify at least one genre.")
            .Must(ids =>
            {
                var existingGenres = dbContext.Genres
                    .Where(g => ids.Contains(g.Id))
                    .ToList();

                return existingGenres.Count == ids.Count;
            }).WithMessage("Invalid Genre ID sent.");

        RuleFor(x => x.DirectorId)
            .NotNull()
            .Must(CustomerId => dbContext.Directors.Any(x => x.Id == CustomerId))
            .WithMessage("Invalid DirectorId, please enter a valid DirectorId!");

        RuleFor(x => x.ActorIds)
            .NotEmpty().WithMessage("You must specify at least one actor.")
            .Must(ids =>
            {
                var existingActors = dbContext.Actors
                    .Where(a => ids.Contains(a.Id))
                    .ToList();

                return existingActors.Count == ids.Count;
            }).WithMessage("Invalid Actor ID sent.");
    }
}

public class UpdateMovieValidator : AbstractValidator<MovieUpdateRequest>
{

    public UpdateMovieValidator(DataContext dbContext)
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50);

        RuleFor(x => x.Year)
           .GreaterThan(1900).WithMessage("Year must be greater than 1900.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0.");

        RuleFor(x => x.GenreIds)
            .NotEmpty().WithMessage("You must specify at least one genre.")
            .Must(ids =>
            {
                var existingGenres = dbContext.Genres
                    .Where(g => ids.Contains(g.Id))
                    .ToList();

                return existingGenres.Count == ids.Count;
            }).WithMessage("Invalid Genre ID sent.");

        RuleFor(x => x.DirectorId)
            .NotNull()
            .Must(CustomerId => dbContext.Directors.Any(x => x.Id == CustomerId))
            .WithMessage("Invalid DirectorId, please enter a valid DirectorId!");

        RuleFor(x => x.ActorIds)
            .NotEmpty().WithMessage("You must specify at least one actor.")
            .Must(ids =>
            {
                var existingActors = dbContext.Actors
                    .Where(a => ids.Contains(a.Id))
                    .ToList();

                return existingActors.Count == ids.Count;
            }).WithMessage("Invalid Actor ID sent.");
    }
}