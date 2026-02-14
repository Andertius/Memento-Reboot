using System.Collections.Generic;
using System.Linq;
using Memento.API.Interfaces;
using Memento.Services.Models;

namespace Memento.API.Endpoints.Cards.AddCard;

public sealed class AddCardRequest : IRequest<Card>
{
    public string? Word { get; set; }
    public string? Translation { get; set; }
    public string? Definition { get; set; }
    public string? Hint { get; set; }
    public IReadOnlyCollection<int> CategoryIds { get; set; } = [];
    public IReadOnlyCollection<int> TagIds { get; set; } = [];

    public Card ToModel() => new()
    {
        Word = Word,
        Translation = Translation,
        Definition = Definition,
        Hint = Hint,
        Categories = CategoryIds.Select(id => new Category() { Id = id }).ToArray(),
        Tags = TagIds.Select(id => new Tag() { Id = id }).ToArray(),
    };
}
