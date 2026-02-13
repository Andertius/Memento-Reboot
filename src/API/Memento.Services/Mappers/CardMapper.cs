using Riok.Mapperly.Abstractions;
using Memento.Domain.Models;
using Memento.Infrastructure.Entities;

namespace Memento.Services.Mappers;

[Mapper(UseDeepCloning = true, RequiredMappingStrategy = RequiredMappingStrategy.None)]
public partial class CardMapper
{
    public partial CardEntity MapCardToCardEntity(Card card);

    public partial Card MapCardEntityToCard(CardEntity card);
}
