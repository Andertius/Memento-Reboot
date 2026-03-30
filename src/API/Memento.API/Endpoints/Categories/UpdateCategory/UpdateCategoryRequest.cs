using Memento.API.Interfaces;
using Memento.Services.Models;

namespace Memento.API.Endpoints.Categories.UpdateCategory;

public sealed class UpdateCategoryRequest : IEntityRequest<Category>
{
    public int Id { get; set; }
    public required string Name { get; init; }
    public required string Description { get; init; }

    public Category ToModel() => new()
    {
        Id = Id,
        Name = Name,
        Description = Description,
    };
}
