using System.Collections.Generic;

namespace Memento.API.Endpoints.Cards.UpdateCardCategories;

public sealed class UpdateCardCategoriesRequest
{
    public int CardId { get; init; }
    public IReadOnlyCollection<int> CategoryIds { get; init; } = [];
}
