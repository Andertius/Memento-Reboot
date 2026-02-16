using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Memento.API.Constants;
using Memento.Services.Services;

namespace Memento.API.Endpoints.Categories.GetCardsByCategoryId;

public sealed class GetCardsByCategoryIdEndpoint(ICardService cardService) : Endpoint<GetCardsByCategoryIdRequest>
{
    private readonly ICardService _cardService = cardService ?? throw new ArgumentNullException(nameof(cardService), "Card Service must not be null");

    public override void Configure()
    {
        Get(ApiPrefixes.CategoriesPrefix + "/{CategoryId}/cards");
        Roles("Learner");
    }

    public override async Task HandleAsync(GetCardsByCategoryIdRequest request, CancellationToken token)
    {
        var cards = await _cardService.FetchByCategory(request.CategoryId, token);
        await Send.OkAsync(cards, cancellation: token);
    }
}
