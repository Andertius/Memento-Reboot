using Memento.Infrastructure.Entities;
using Memento.Services.Models;
using Riok.Mapperly.Abstractions;

namespace Memento.Services.Mappers;

[Mapper(UseDeepCloning = true, RequiredMappingStrategy = RequiredMappingStrategy.None)]
public partial class CardMapper
{
    public partial CardEntity MapCardToCardEntity(Card card);

    public partial Card MapCardEntityToCard(CardEntity card);
}
