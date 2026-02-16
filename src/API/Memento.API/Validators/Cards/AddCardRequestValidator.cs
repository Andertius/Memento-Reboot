using FastEndpoints;
using FluentValidation;
using Memento.API.Endpoints.Cards.AddCard;

namespace Memento.API.Validators.Cards;

public sealed class AddCardRequestValidator : Validator<AddCardRequest>
{
    public AddCardRequestValidator()
    {
        RuleFor(x => x.Word)
            .NotEmpty()
            .WithMessage("Card word is required")
            .MaximumLength(256)
            .WithMessage("Card word cannot be longer than 256 characters");

        RuleFor(x => x.Translation)
            .NotEmpty()
            .WithMessage("Card translation is required")
            .MaximumLength(256)
            .WithMessage("Card translation cannot be longer than 256 characters");

        RuleFor(x => x.Definition)
            .MaximumLength(256)
            .WithMessage("Card definition cannot be longer than 256 characters");

        RuleFor(x => x.Hint)
            .MaximumLength(256)
            .WithMessage("Card hint cannot be longer than 256 characters");
    }
}
