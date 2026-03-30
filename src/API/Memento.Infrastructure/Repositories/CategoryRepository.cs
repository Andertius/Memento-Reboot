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

    Task UpdateCategory(CategoryEntity entity, CancellationToken token = default);

    Task<(bool Exists, string? ImageName)> GetImageName(int categoryId, CancellationToken token = default);

    Task UpsertImage(int categoryId, string imageName, CancellationToken token = default);

    Task RemoveImage(int categoryId, CancellationToken token = default);

    Task<CategoryEntity?> GetById(int id, CancellationToken token = default);

    Task<CategoryEntity?> GetByName(string? name, CancellationToken token = default);

    Task RemoveCategory(int id, CancellationToken token = default);

    Task UpdateCategoryCards(int categoryId, IReadOnlyCollection<int> cardIds, CancellationToken token = default);

    Task UpdateCategoryTags(int categoryId, IReadOnlyCollection<int> tagIds, CancellationToken token = default);
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

    public async Task UpdateCategory(CategoryEntity entity, CancellationToken token = default)
    {
        _context.Categories.Update(entity);
        _context.Entry(entity).Property(x => x.Image).IsModified = false;
        await _context.SaveChangesAsync(token);
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

    public async Task<(bool Exists, string? ImageName)> GetImageName(int categoryId, CancellationToken token = default)
    {
        var category = await _context
            .Categories
            .AsNoTracking()
            .Select(x => new { x.Id, x.Image })
            .FirstOrDefaultAsync(x => x.Id == categoryId, token);

        return (category is not null, category?.Image);
    }

    public async Task UpsertImage(int categoryId, string imageName, CancellationToken token = default)
    {
        var entity = await _context.Categories.FindAsync([categoryId], token);

        if (entity is null)
        {
            return;
        }
        
        entity.Image = imageName;
        await _context.SaveChangesAsync(token);
    }

    public async Task RemoveImage(int categoryId, CancellationToken token = default)
    {
        var entity = await _context.Categories.FindAsync([categoryId], token);

        if (entity is null)
        {
            return;
        }
        
        entity.Image = null;
        await _context.SaveChangesAsync(token);
    }

    public async Task UpdateCategoryCards(int categoryId, IReadOnlyCollection<int> cardIds, CancellationToken token = default)
    {
        var categoryEntity = await _context
            .Categories
            .Include(x => x.Cards)
            .FirstOrDefaultAsync(x => x.Id == categoryId, token);

        if (categoryEntity is null)
        {
            return;
        }

        categoryEntity.Cards = await _context
            .Cards
            .Where(x => cardIds.Contains(x.Id))
            .ToListAsync(token);

        await _context.SaveChangesAsync(token);
    }

    public async Task UpdateCategoryTags(int categoryId, IReadOnlyCollection<int> tagIds, CancellationToken token = default)
    {
        var categoryEntity = await _context
            .Categories
            .Include(x => x.Tags)
            .FirstOrDefaultAsync(x => x.Id == categoryId, token);

        if (categoryEntity is null)
        {
            return;
        }

        categoryEntity.Tags = await _context
            .Tags
            .Where(x => tagIds.Contains(x.Id))
            .ToListAsync(token);

        await _context.SaveChangesAsync(token);
    }
}
