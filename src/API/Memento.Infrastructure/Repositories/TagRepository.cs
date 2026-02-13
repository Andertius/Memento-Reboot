using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Memento.Infrastructure.Database;
using Memento.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Memento.Infrastructure.Repositories;

public interface ITagRepository
{
    Task<TagEntity[]> GetAllTags();
    Task<int> AddTag(TagEntity entity);
    Task<TagEntity?> GetById(int id);
    Task RemoveTag(int id);
    Task AddTagsToCard(int cardId, IReadOnlyCollection<int> tagIds);
    Task RemoveTagFromCard(int tagId, int cardId);
    Task AddTagsToCategory(int categoryId, IReadOnlyCollection<int> tagIds);
    Task RemoveTagFromCategory(int tagId, int categoryId);
}

public sealed class TagRepository(CardDbContext context) : ITagRepository
{
    private readonly CardDbContext _context = context ?? throw new ArgumentNullException("Card DbContext must not be null", nameof(context));

    public Task<TagEntity[]> GetAllTags()
        => _context
            .Tags
            .AsNoTracking()
            .ToArrayAsync();

    public async Task<int> AddTag(TagEntity entity)
    {
        _context.Tags.Add(entity);
        await _context.SaveChangesAsync();
        return entity.Id;
    }

    public Task<TagEntity?> GetById(int id)
        => _context
            .Tags
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

    public async Task RemoveTag(int id)
    {
        var tag = await _context
            .Tags
            .FindAsync(id);

        if (tag is null)
        {
            return;
        }

        _context.Tags.Remove(tag);
        await _context.SaveChangesAsync();
    }

    public async Task AddTagsToCard(int cardId, IReadOnlyCollection<int> tagIds)
    {
        var cardEntity = await _context.Cards.FindAsync(cardId);

        if (cardEntity is null)
        {
            return;
        }

        var tagEntities = await _context
            .Tags
            .Where(x => tagIds.Contains(x.Id))
            .ToArrayAsync();

        foreach (var tag in tagEntities)
        {
            cardEntity.Tags.Add(tag);
        }

        await _context.SaveChangesAsync();
    }

    public async Task RemoveTagFromCard(int tagId, int cardId)
    {
        var tagEntity = await _context.Tags.FindAsync(tagId);

        if (tagEntity is null)
        {
            return;
        }

        var cardEntity = await _context
            .Cards
            .Include(x => x.Tags)
            .FirstOrDefaultAsync(x => cardId == x.Id);

        if (cardEntity is null)
        {
            return;
        }

        cardEntity.Tags.Remove(tagEntity);
        await _context.SaveChangesAsync();
    }

    public async Task AddTagsToCategory(int categoryId, IReadOnlyCollection<int> tagIds)
    {
        var categoryEntity = await _context.Categories.FindAsync(categoryId);

        if (categoryEntity is null)
        {
            return;
        }

        var tagEntities = await _context
            .Tags
            .Where(x => tagIds.Contains(x.Id))
            .ToArrayAsync();

        foreach (var tag in tagEntities)
        {
            categoryEntity.Tags.Add(tag);
        }

        await _context.SaveChangesAsync();
    }

    public async Task RemoveTagFromCategory(int tagId, int categoryId)
    {
        var tagEntity = await _context.Tags.FindAsync(tagId);

        if (tagEntity is null)
        {
            return;
        }

        var categoryEntity = await _context
            .Categories
            .Include(x => x.Tags)
            .FirstOrDefaultAsync(x => categoryId == x.Id);

        if (categoryEntity is null)
        {
            return;
        }

        categoryEntity.Tags.Remove(tagEntity);
        await _context.SaveChangesAsync();
    }
}
