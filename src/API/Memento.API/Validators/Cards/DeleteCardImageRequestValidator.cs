using FastEndpoints;
using FluentValidation;
using Memento.API.Endpoints.Cards.DeleteCardImage;

namespace Memento.API.Validators.Cards;

public sealed class DeleteCardImageRequestValidator : Validator<DeleteCardImageRequest>
{
    public DeleteCardImageRequestValidator()
    {
        RuleFor(x => x.CardId)
            .GreaterThan(0)
            .WithMessage("CardId must be a positive integer");
    }
}
