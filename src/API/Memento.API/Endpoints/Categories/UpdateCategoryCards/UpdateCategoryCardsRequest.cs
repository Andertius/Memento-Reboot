using System.Collections.Generic;

namespace Memento.API.Endpoints.Categories.UpdateCategoryCards;

public sealed class UpdateCategoryCardsRequest
{
    public int CategoryId { get; init; }
    public IReadOnlyCollection<int> CardIds { get; init; } = [];
}
