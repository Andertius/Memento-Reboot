using System.Collections.Generic;
using System.Linq;
using Memento.API.Interfaces;
using Memento.Domain.Models;

namespace Memento.API.Endpoints.Categories.AddCategory;

public sealed class AddCategoryRequest : IRequest<Category>
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public IReadOnlyCollection<int> TagIds { get; set; } = [];

    public Category ToModel() => new()
    {
        Name = Name,
        Description = Description,
        Tags = TagIds.Select(id => new Tag() { Id = id }).ToArray(),
    };
}
