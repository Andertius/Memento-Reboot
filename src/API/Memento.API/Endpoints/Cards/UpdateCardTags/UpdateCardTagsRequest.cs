using System.Collections.Generic;

namespace Memento.API.Endpoints.Cards.UpdateCardTags;

public sealed class UpdateCardTagsRequest
{
    public int CardId { get; init; }
    public IReadOnlyCollection<int> TagIds { get; init; } = [];
}
