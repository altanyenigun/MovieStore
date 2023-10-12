using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MovieStoreApi.DTOs;

namespace MovieStoreApi.Operation.Validation;

public class CreateDirectorValidator : AbstractValidator<DirectorCreateRequest>
{

    public CreateDirectorValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Surname).NotEmpty().MaximumLength(50);
    }
}

public class UpdateDirectorValidator : AbstractValidator<DirectorUpdateRequest>
{

    public UpdateDirectorValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Surname).NotEmpty().MaximumLength(50);
    }
}