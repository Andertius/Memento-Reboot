using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Memento.Infrastructure.Repositories;
using Memento.Services.Mappers;
using Memento.Services.Models;

namespace Memento.Services.Services;

public interface ICategoryService
{
    Task<Category[]> GetAllCategories(CancellationToken token = default);

    Task<int> AddCategory(Category category, CancellationToken token = default);

    Task<bool> UpdateCategory(Category category, CancellationToken token = default);

    Task<Category?> GetById(int id, CancellationToken token = default);

    Task<Category?> GetByName(string name, CancellationToken token = default);

    Task RemoveCategory(int id, CancellationToken token = default);

    Task UpdateCategoryCards(int categoryId, IReadOnlyCollection<int> cardIds, CancellationToken token = default);
    
    Task UpdateCategoryTags(int categoryId, IReadOnlyCollection<int> tagIds, CancellationToken token = default);
}

public sealed class CategoryService(ICategoryRepository categoryRepository) : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository), "Category Repository must not be null");
    private readonly CategoryMapper _categoryMapper = new();

    public async Task<Category[]> GetAllCategories(CancellationToken token = default)
    {
        var cards = await _categoryRepository.GetAllCategories(token);

        return cards.Select(_categoryMapper.MapCategoryEntityToCategory).ToArray();
    }

    public async Task<int> AddCategory(Category category, CancellationToken token = default)
    {
        var existing = await _categoryRepository.GetByName(category.Name, token);

        if (existing is not null)
        {
            return 0;
        }

        var categoryEntity = _categoryMapper.MapCategoryToCategoryEntity(category);

        return await _categoryRepository.AddCategory(categoryEntity, token);
    }

    public async Task<bool> UpdateCategory(Category category, CancellationToken token = default)
    {
        var existing = await _categoryRepository.GetByName(category.Name, token);

        if (existing is not null && existing.Id != category.Id)
        {
            return false;
        }

        var categoryEntity = _categoryMapper.MapCategoryToCategoryEntity(category);
        await _categoryRepository.UpdateCategory(categoryEntity, token);

        return true;
    }

    public async Task<Category?> GetById(int id, CancellationToken token = default)
    {
        var category = await _categoryRepository.GetById(id, token);

        return category is null
            ? null
            : _categoryMapper.MapCategoryEntityToCategory(category);
    }

    public async Task<Category?> GetByName(string name, CancellationToken token = default)
    {
        var category = await _categoryRepository.GetByName(name, token);

        return category is null
            ? null
            : _categoryMapper.MapCategoryEntityToCategory(category);
    }

    public Task RemoveCategory(int id, CancellationToken token = default)
        => _categoryRepository.RemoveCategory(id, token);

    public Task UpdateCategoryCards(int categoryId, IReadOnlyCollection<int> cardIds, CancellationToken token = default)
        => _categoryRepository.UpdateCategoryCards(categoryId, cardIds, token);

    public Task UpdateCategoryTags(int categoryId, IReadOnlyCollection<int> tagIds, CancellationToken token = default)
        => _categoryRepository.UpdateCategoryTags(categoryId, tagIds, token);
}
