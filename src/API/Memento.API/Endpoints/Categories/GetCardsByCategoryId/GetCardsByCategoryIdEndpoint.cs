using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Memento.Services.Services;

namespace Memento.API.Endpoints.Categories.GetCardsByCategoryId;

public sealed class GetCardsByCategoryIdEndpoint(ICardService cardService) : Endpoint<GetCardsByCategoryIdRequest>
{
    private readonly ICardService _cardService = cardService ?? throw new ArgumentNullException("Card Service must not be null", nameof(cardService));

    public override void Configure()
    {
        Get("/api/categories/{CategoryId}/cards");
        Roles("Learner");
    }

    public override async Task HandleAsync(GetCardsByCategoryIdRequest request, CancellationToken token)
    {
        var cards = await _cardService.FetchByCategory(request.CategoryId);
        await Send.OkAsync(cards);
    }
}
