using System.Collections.Generic;

namespace Memento.API.Endpoints.Categories.AddCardsToCategory;

public sealed class AddCardsToCategoryRequest
{
    public int CategoryId { get; init; }
    public IReadOnlyCollection<int> CardIds { get; init; } = [];
}
