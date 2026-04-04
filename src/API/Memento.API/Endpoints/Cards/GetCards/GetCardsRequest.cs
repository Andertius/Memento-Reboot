using System.Collections.Generic;

namespace Memento.API.Endpoints.Cards.GetCards;

public sealed class GetCardsRequest
{
    public int? CategoryId { get; init; }
    public IReadOnlyCollection<int>? TagIds { get; init; }
}
