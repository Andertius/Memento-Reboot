using System.Collections.Generic;

namespace Memento.API.Endpoints.Categories.AddTagsToCategory;

public sealed class AddTagsToCategoryRequest
{
    public int CategoryId { get; init; }
    public IReadOnlyCollection<int> TagIds { get; init; } = [];
}
