using FastEndpoints;
using FluentValidation;
using Memento.API.Endpoints.Cards.UploadCardImage;

namespace Memento.API.Validators.Cards;

public sealed class UploadCardImageRequestValidator : Validator<UploadCardImageRequest>
{
    public UploadCardImageRequestValidator()
    {
        RuleFor(x => x.CardId)
            .GreaterThan(0)
            .WithMessage("CardId must be a positive integer");
    }
}
