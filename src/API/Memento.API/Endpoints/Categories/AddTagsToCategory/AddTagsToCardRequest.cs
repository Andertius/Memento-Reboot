using System.Collections.Generic;

namespace Memento.API.Endpoints.Cards.AddTagsToCategory;

public sealed class AddTagsToCategoryRequest
{
    public int CategoryId { get; set; }
    public IReadOnlyCollection<int> TagIds { get; set; } = [];
}
