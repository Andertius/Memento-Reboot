using System.Collections.Generic;
using System.Linq;
using Memento.API.Interfaces;
using Memento.Services.Models;

namespace Memento.API.Endpoints.Categories.AddCategory;

public sealed class AddCategoryRequest : IEntityRequest<Category>
{
    public string Name { get; init; } = "";
    public string Description { get; init; } = "";
    public IReadOnlyCollection<int> TagIds { get; set; } = [];

    public Category ToModel() => new()
    {
        Name = Name,
        Description = Description,
        Tags = TagIds.Select(id => new Tag { Id = id }).ToArray(),
    };
}
