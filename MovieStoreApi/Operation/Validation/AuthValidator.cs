using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MovieStoreApi.DTOs;

namespace MovieStoreApi.Operation.Validation;

public class CustomerRegisterValidator : AbstractValidator<CustomerRegisterRequest>
{

    public CustomerRegisterValidator()
    {
        RuleFor(x => x.Username).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Surname).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Password).NotEmpty().MaximumLength(50);
    }
}