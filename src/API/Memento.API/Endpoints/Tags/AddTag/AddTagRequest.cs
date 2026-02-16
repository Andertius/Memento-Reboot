using Memento.API.Interfaces;
using Memento.Services.Models;

namespace Memento.API.Endpoints.Tags.AddTag;

public sealed class AddTagRequest : IEntityRequest<Tag>
{
    public string Name { get; init; } = "";

    public Tag ToModel()
        => new() { Name = Name };
}
