using Memento.API.Interfaces;
using Memento.Domain.Models;

namespace Memento.API.Endpoints.Tags.AddTag;

public sealed class AddTagRequest : IRequest<Tag>
{
    public string? Name { get; set; }

    public Tag ToModel() => new()
    {
        Name = Name,
    };
}
