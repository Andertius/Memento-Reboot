using System.Collections.Generic;
using System.Linq;
using Memento.API.Interfaces;
using Memento.Services.Models;

namespace Memento.API.Endpoints.Cards.AddCard;

public sealed class AddCardRequest : IEntityRequest<Card>
{
    public required string Word { get; init; }
    public required string Translation { get; init; }
    public string? Definition { get; init; }
    public string? Hint { get; init; }
    public IReadOnlyCollection<int> CategoryIds { get; init; } = [];
    public IReadOnlyCollection<int> TagIds { get; init; } = [];

    public Card ToModel() => new()
    {
        Word = Word,
        Translation = Translation,
        Definition = Definition,
        Hint = Hint,
        Categories = CategoryIds.Select(id => new Category { Id = id }).ToArray(),
        Tags = TagIds.Select(id => new Tag { Id = id }).ToArray(),
    };
}
