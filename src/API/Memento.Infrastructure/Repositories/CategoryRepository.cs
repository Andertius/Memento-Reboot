using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Memento.Infrastructure.Database;
using Memento.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Memento.Infrastructure.Repositories;

public interface ICategoryRepository
{
    Task<CategoryEntity[]> GetAllCategories(CancellationToken token = default);
    Task<int> AddCategory(CategoryEntity entity, CancellationToken token = default);
    Task<CategoryEntity?> GetById(int id, CancellationToken token = default);
    Task<CategoryEntity?> GetByName(string? name, CancellationToken token = default);
    Task RemoveCategory(int id, CancellationToken token = default);
    Task AddCardsToCategory(int categoryId, IReadOnlyCollection<int> cardIds, CancellationToken token = default);
    Task RemoveCardFromCategory(int categoryId, int cardId, CancellationToken token = default);
}

public sealed class CategoryRepository(CardDbContext context) : ICategoryRepository
{
    private readonly CardDbContext _context = context ?? throw new ArgumentNullException(nameof(context), "Card DbContext must not be null");

    public Task<CategoryEntity[]> GetAllCategories(CancellationToken token = default)
        => _context
            .Categories
            .AsNoTracking()
            .Include(x => x.Tags)
            .ToArrayAsync(token);

    public async Task<int> AddCategory(CategoryEntity entity, CancellationToken token = default)
    {
        entity.Tags = await _context
            .Tags
            .Where(x => entity.Tags.Select(x => x.Id).Contains(x.Id))
            .ToArrayAsync(token);

        _context.Categories.Add(entity);
        await _context.SaveChangesAsync(token);
        return entity.Id;
    }

    public Task<CategoryEntity?> GetById(int id, CancellationToken token = default)
        => _context
            .Categories
            .AsNoTracking()
            .Include(x => x.Tags)
            .FirstOrDefaultAsync(x => x.Id == id, token);

    public Task<CategoryEntity?> GetByName(string? name, CancellationToken token = default)
        => _context
            .Categories
            .AsNoTracking()
            .Include(x => x.Tags)
            .FirstOrDefaultAsync(x => x.Name == name, token);
    
    public async Task RemoveCategory(int id, CancellationToken token = default)
    {
        var category = await _context
            .Categories
            .FindAsync([id], token);

        if (category is null)
        {
            return;
        }

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync(token);
    }

    public async Task AddCardsToCategory(int categoryId, IReadOnlyCollection<int> cardIds, CancellationToken token = default)
    {
        var categoryEntity = await _context.Categories.FindAsync([categoryId], token);

        if (categoryEntity is null)
        {
            return;
        }

        var cardEntities = await _context
            .Cards
            .Include(x => x.Categories)
            .Where(x => cardIds.Contains(x.Id))
            .ToArrayAsync(token);

        foreach (var card in cardEntities)
        {
            card.Categories.Add(categoryEntity);
        }

        await _context.SaveChangesAsync(token);
    }

    public async Task RemoveCardFromCategory(int categoryId, int cardId, CancellationToken token = default)
    {
        var categoryEntity = await _context.Categories.FindAsync([categoryId], token);

        if (categoryEntity is null)
        {
            return;
        }

        var cardEntity = await _context
            .Cards
            .Include(x => x.Categories)
            .FirstOrDefaultAsync(x => cardId == x.Id, token);

        if (cardEntity is null)
        {
            return;
        }

        cardEntity.Categories.Remove(categoryEntity);
        await _context.SaveChangesAsync(token);
    }
}
