using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Memento.Infrastructure.Repositories;
using Memento.Services.Mappers;
using Memento.Services.Models;

namespace Memento.Services.Services;

public interface ICategoryService
{
    Task<Category[]> GetAllCategories();
    Task<int> AddCategory(Category category);
    Task<Category?> GetById(int id);
    Task RemoveCategory(int id);
    Task AddCardsToCategory(int categoryId, IReadOnlyCollection<int> cardIds);
    Task RemoveCardFromCategory(int categoryId, int cardId);
}

public sealed class CategoryService(ICategoryRepository categoryRepository) : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository), "Category Repository must not be null");
    private readonly CategoryMapper _categoryMapper = new();

    public async Task<Category[]> GetAllCategories()
    {
        var cards = await _categoryRepository.GetAllCategories();
        return cards.Select(_categoryMapper.MapCategoryEntityToCategory).ToArray();
    }

    public async Task<int> AddCategory(Category category)
    {
        var categoryEntity = _categoryMapper.MapCategoryToCategoryEntity(category);
        return await _categoryRepository.AddCard(categoryEntity);
    }

    public async Task<Category?> GetById(int id)
    {
        var category = await _categoryRepository.GetById(id);

        if (category is null)
        {
            return null;
        }

        return _categoryMapper.MapCategoryEntityToCategory(category);
    }

    public Task RemoveCategory(int id)
        => _categoryRepository.RemoveCategory(id);

    public Task AddCardsToCategory(int categoryId, IReadOnlyCollection<int> cardIds)
        => _categoryRepository.AddCardsToCategory(categoryId, cardIds);

    public Task RemoveCardFromCategory(int categoryId, int cardId)
        => _categoryRepository.RemoveCardFromCategory(categoryId, cardId);
}
