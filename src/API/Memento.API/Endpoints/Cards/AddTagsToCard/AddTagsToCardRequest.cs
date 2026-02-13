using System.Collections.Generic;

namespace Memento.API.Endpoints.Cards.AddTagsToCard;

public sealed class AddTagsToCardRequest
{
    public int CardId { get; set; }
    public IReadOnlyCollection<int> TagIds { get; set; } = [];
}
