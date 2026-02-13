using Riok.Mapperly.Abstractions;
using Memento.Domain.Models;
using Memento.Infrastructure.Entities;

namespace Memento.Services.Mappers;

[Mapper(UseDeepCloning = true, RequiredMappingStrategy = RequiredMappingStrategy.None)]
public partial class TagMapper
{
    [MapperIgnoreTarget(nameof(TagEntity.Cards))]
    [MapperIgnoreTarget(nameof(TagEntity.Categories))]
    public partial TagEntity MapTagToTagEntity(Tag tag);

    [MapperIgnoreSource(nameof(TagEntity.Cards))]
    [MapperIgnoreSource(nameof(TagEntity.Categories))]
    public partial Tag MapTagEntityToTag(TagEntity tag);
}
