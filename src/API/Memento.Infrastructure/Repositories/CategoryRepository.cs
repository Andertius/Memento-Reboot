using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Memento.Infrastructure.Database;
using Memento.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Memento.Infrastructure.Repositories;

public interface ICategoryRepository
{
    Task<CategoryEntity[]> GetAllCategories();
    Task<int> AddCard(CategoryEntity entity);
    Task<CategoryEntity?> GetById(int id);
    Task<CategoryEntity?> GetByName(string? name);
    Task RemoveCategory(int id);
    Task AddCardsToCategory(int categoryId, IReadOnlyCollection<int> cardIds);
    Task RemoveCardFromCategory(int categoryId, int cardId);
}

public sealed class CategoryRepository(CardDbContext context) : ICategoryRepository
{
    private readonly CardDbContext _context = context ?? throw new ArgumentNullException(nameof(context), "Card DbContext must not be null");

    public Task<CategoryEntity[]> GetAllCategories()
        => _context
            .Categories
            .AsNoTracking()
            .Include(x => x.Tags)
            .ToArrayAsync();

    public async Task<int> AddCard(CategoryEntity entity)
    {
        entity.Tags = await _context
            .Tags
            .Where(x => entity.Tags.Select(x => x.Id).Contains(x.Id))
            .ToArrayAsync();

        _context.Categories.Add(entity);
        await _context.SaveChangesAsync();
        return entity.Id;
    }

    public Task<CategoryEntity?> GetById(int id)
        => _context
            .Categories
            .AsNoTracking()
            .Include(x => x.Tags)
            .FirstOrDefaultAsync(x => x.Id == id);

    public Task<CategoryEntity?> GetByName(string? name)
        => _context
            .Categories
            .AsNoTracking()
            .Include(x => x.Tags)
            .FirstOrDefaultAsync(x => x.Name == name);
    
    public async Task RemoveCategory(int id)
    {
        var category = await _context
            .Categories
            .FindAsync(id);

        if (category is null)
        {
            return;
        }

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
    }

    public async Task AddCardsToCategory(int categoryId, IReadOnlyCollection<int> cardIds)
    {
        var categoryEntity = await _context.Categories.FindAsync(categoryId);

        if (categoryEntity is null)
        {
            return;
        }

        var cardEntities = await _context
            .Cards
            .Include(x => x.Categories)
            .Where(x => cardIds.Contains(x.Id))
            .ToArrayAsync();

        foreach (var card in cardEntities)
        {
            card.Categories.Add(categoryEntity);
        }

        await _context.SaveChangesAsync();
    }

    public async Task RemoveCardFromCategory(int categoryId, int cardId)
    {
        var categoryEntity = await _context.Categories.FindAsync(categoryId);

        if (categoryEntity is null)
        {
            return;
        }

        var cardEntity = await _context
            .Cards
            .Include(x => x.Categories)
            .FirstOrDefaultAsync(x => cardId == x.Id);

        if (cardEntity is null)
        {
            return;
        }

        cardEntity.Categories.Remove(categoryEntity);
        await _context.SaveChangesAsync();
    }
}
