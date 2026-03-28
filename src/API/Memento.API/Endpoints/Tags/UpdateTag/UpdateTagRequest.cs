using Memento.API.Interfaces;
using Memento.Services.Models;

namespace Memento.API.Endpoints.Tags.UpdateTag;

public sealed class UpdateTagRequest : IEntityRequest<Tag>
{
    public int Id { get; set; }
    public required string Name { get; init; }

    public Tag ToModel() => new()
    {
        Id = Id,
        Name = Name,
    };
}
