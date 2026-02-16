using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Memento.Infrastructure.Database;
using Memento.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Memento.Infrastructure.Repositories;

public interface ITagRepository
{
    Task<TagEntity[]> GetAllTags(CancellationToken token = default);
    Task<int> AddTag(TagEntity entity, CancellationToken token = default);
    Task<TagEntity?> GetById(int id, CancellationToken token = default);
    Task<TagEntity?> GetByName(string? name, CancellationToken token = default);
    Task RemoveTag(int id, CancellationToken token = default);
    Task AddTagsToCard(int cardId, IReadOnlyCollection<int> tagIds, CancellationToken token = default);
    Task RemoveTagFromCard(int tagId, int cardId, CancellationToken token = default);
    Task AddTagsToCategory(int categoryId, IReadOnlyCollection<int> tagIds, CancellationToken token = default);
    Task RemoveTagFromCategory(int tagId, int categoryId, CancellationToken token = default);
}

public sealed class TagRepository(CardDbContext context) : ITagRepository
{
    private readonly CardDbContext _context = context ?? throw new ArgumentNullException(nameof(context), "Card DbContext must not be null");

    public Task<TagEntity[]> GetAllTags(CancellationToken token = default)
        => _context
            .Tags
            .AsNoTracking()
            .ToArrayAsync(token);

    public async Task<int> AddTag(TagEntity entity, CancellationToken token = default)
    {
        _context.Tags.Add(entity);
        await _context.SaveChangesAsync(token);
        return entity.Id;
    }

    public Task<TagEntity?> GetById(int id, CancellationToken token = default)
        => _context
            .Tags
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, token);

    public Task<TagEntity?> GetByName(string? name, CancellationToken token = default)
        => _context
            .Tags
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Name == name, token);

    public async Task RemoveTag(int id, CancellationToken token = default)
    {
        var tag = await _context
            .Tags
            .FindAsync([id], token);

        if (tag is null)
        {
            return;
        }

        _context.Tags.Remove(tag);
        await _context.SaveChangesAsync(token);
    }

    public async Task AddTagsToCard(int cardId, IReadOnlyCollection<int> tagIds, CancellationToken token = default)
    {
        var cardEntity = await _context.Cards.FindAsync([cardId], token);

        if (cardEntity is null)
        {
            return;
        }

        var tagEntities = await _context
            .Tags
            .Where(x => tagIds.Contains(x.Id))
            .ToArrayAsync(token);

        foreach (var tag in tagEntities)
        {
            cardEntity.Tags.Add(tag);
        }

        await _context.SaveChangesAsync(token);
    }

    public async Task RemoveTagFromCard(int tagId, int cardId, CancellationToken token = default)
    {
        var tagEntity = await _context.Tags.FindAsync([tagId], token);

        if (tagEntity is null)
        {
            return;
        }

        var cardEntity = await _context
            .Cards
            .Include(x => x.Tags)
            .FirstOrDefaultAsync(x => cardId == x.Id, token);

        if (cardEntity is null)
        {
            return;
        }

        cardEntity.Tags.Remove(tagEntity);
        await _context.SaveChangesAsync(token);
    }

    public async Task AddTagsToCategory(int categoryId, IReadOnlyCollection<int> tagIds, CancellationToken token = default)
    {
        var categoryEntity = await _context.Categories.FindAsync([categoryId], token);

        if (categoryEntity is null)
        {
            return;
        }

        var tagEntities = await _context
            .Tags
            .Where(x => tagIds.Contains(x.Id))
            .ToArrayAsync(token);

        foreach (var tag in tagEntities)
        {
            categoryEntity.Tags.Add(tag);
        }

        await _context.SaveChangesAsync(token);
    }

    public async Task RemoveTagFromCategory(int tagId, int categoryId, CancellationToken token = default)
    {
        var tagEntity = await _context.Tags.FindAsync([tagId], token);

        if (tagEntity is null)
        {
            return;
        }

        var categoryEntity = await _context
            .Categories
            .Include(x => x.Tags)
            .FirstOrDefaultAsync(x => categoryId == x.Id, token);

        if (categoryEntity is null)
        {
            return;
        }

        categoryEntity.Tags.Remove(tagEntity);
        await _context.SaveChangesAsync(token);
    }
}
