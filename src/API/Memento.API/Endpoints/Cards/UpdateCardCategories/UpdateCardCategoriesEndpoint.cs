using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Memento.API.Constants;
using Memento.Services.Services;

namespace Memento.API.Endpoints.Cards.UpdateCardCategories;

public sealed class UpdateCardCategoriesEndpoint(ICardService cardService) : Endpoint<UpdateCardCategoriesRequest>
{
    private readonly ICardService _cardService = cardService ?? throw new ArgumentNullException(nameof(cardService), "Card Service must not be null");

    public override void Configure()
    {
        Put(ApiPrefixes.CardsApiPrefix + "/{CardId}/categories");
        Roles("Learner");
    }

    public override async Task HandleAsync(UpdateCardCategoriesRequest request, CancellationToken token)
    {
        await _cardService.UpdateCardCategories(request.CardId, request.CategoryIds, token);
        await Send.OkAsync(cancellation: token);
    }
}
