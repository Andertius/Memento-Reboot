using System.Collections.Generic;

namespace Memento.API.Endpoints.Categories.AddTagsToCategory;

public sealed class AddTagsToCategoryRequest
{
    public int CategoryId { get; set; }
    public IReadOnlyCollection<int> TagIds { get; set; } = [];
}
