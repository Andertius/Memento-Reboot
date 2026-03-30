using System.Collections.Generic;

namespace Memento.API.Endpoints.Categories.UpdateCategoryTags;

public sealed class UpdateCategoryTagsRequest
{
    public int CategoryId { get; init; }
    public IReadOnlyCollection<int> TagIds { get; init; } = [];
}
