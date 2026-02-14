using Memento.Infrastructure.Entities;
using Memento.Services.Models;
using Riok.Mapperly.Abstractions;

namespace Memento.Services.Mappers;

[Mapper(UseDeepCloning = true, RequiredMappingStrategy = RequiredMappingStrategy.None)]
public partial class CategoryMapper
{
    [MapperIgnoreTarget(nameof(CategoryEntity.Cards))]
    public partial CategoryEntity MapCategoryToCategoryEntity(Category category);

    [MapperIgnoreSource(nameof(CategoryEntity.Cards))]
    public partial Category MapCategoryEntityToCategory(CategoryEntity category);
}
