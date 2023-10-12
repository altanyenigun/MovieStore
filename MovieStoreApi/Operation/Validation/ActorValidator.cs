using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MovieStoreApi.DTOs;

namespace MovieStoreApi.Operation.Validation;

public class CreateActorValidator : AbstractValidator<ActorCreateRequest>
{

    public CreateActorValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Surname).NotEmpty().MaximumLength(50);
    }
}

public class UpdateActorValidator : AbstractValidator<ActorUpdateRequest>
{

    public UpdateActorValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Surname).NotEmpty().MaximumLength(50);
    }
}