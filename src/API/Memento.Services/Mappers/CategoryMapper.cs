using Riok.Mapperly.Abstractions;
using Memento.Domain.Models;
using Memento.Infrastructure.Entities;

namespace Memento.Services.Mappers;

[Mapper(UseDeepCloning = true, RequiredMappingStrategy = RequiredMappingStrategy.None)]
public partial class CategoryMapper
{
    [MapperIgnoreTarget(nameof(CategoryEntity.Cards))]
    public partial CategoryEntity MapCategoryToCategoryEntity(Category category);

    [MapperIgnoreSource(nameof(CategoryEntity.Cards))]
    public partial Category MapCategoryEntityToCategory(CategoryEntity category);
}
