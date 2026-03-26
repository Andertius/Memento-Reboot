using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Memento.Infrastructure.Repositories;
using Memento.Services.Mappers;
using Memento.Services.Models;

namespace Memento.Services.Services;

public interface ITagService
{
    Task<Tag[]> GetAllTags(CancellationToken token = default);
    Task<Tag?> GetTagById(int id, CancellationToken token = default);
    Task<Tag?> GetTagByName(string name, CancellationToken token = default);
    Task<int> AddTag(Tag tag, CancellationToken token = default);
    Task<bool> UpdateTag(Tag tag, CancellationToken token = default);
    Task RemoveTag(int id, CancellationToken token = default);
    Task AddTagsToCard(int cardId, IReadOnlyCollection<int> tagIds, CancellationToken token = default);
    Task RemoveTagFromCard(int tagId, int cardId, CancellationToken token = default);
    Task AddTagsToCategory(int categoryId, IReadOnlyCollection<int> tagIds, CancellationToken token = default);
    Task RemoveTagFromCategory(int tagId, int categoryId, CancellationToken token = default);
}

public sealed class TagService(ITagRepository tagRepository) : ITagService
{
    private readonly ITagRepository _tagRepository = tagRepository ?? throw new ArgumentNullException(nameof(tagRepository), "Tag Repository must not be null");
    private readonly TagMapper _tagMapper = new();

    public async Task<Tag[]> GetAllTags(CancellationToken token = default)
    {
        var cards = await _tagRepository.GetAllTags(token);
        return cards.Select(_tagMapper.MapTagEntityToTag).ToArray();
    }

    public async Task<Tag?> GetTagById(int id, CancellationToken token = default)
    {
        var entity = await _tagRepository.GetById(id, token);

        return entity is null
            ? null
            : _tagMapper.MapTagEntityToTag(entity);
    }

    public async Task<Tag?> GetTagByName(string name, CancellationToken token = default)
    {
        var entity = await _tagRepository.GetByName(name, token);

        return entity is null
            ? null
            : _tagMapper.MapTagEntityToTag(entity);
    }

    public async Task<int> AddTag(Tag tag, CancellationToken token = default)
    {
        var existing = await _tagRepository.GetByName(tag.Name, token);

        if (existing is not null)
        {
            return 0;
        }

        var tagEntity = _tagMapper.MapTagToTagEntity(tag);
        return await _tagRepository.AddTag(tagEntity, token);
    }

    public async Task<bool> UpdateTag(Tag tag, CancellationToken token = default)
    {
        var existing = await _tagRepository.GetByName(tag.Name, token);

        if (existing is not null && existing.Id != tag.Id)
        {
            return false;
        }

        var tagEntity = _tagMapper.MapTagToTagEntity(tag);
        await _tagRepository.UpdateTag(tagEntity, token);

        return true;
    }

    public Task RemoveTag(int id, CancellationToken token = default)
        => _tagRepository.RemoveTag(id, token);

    public Task AddTagsToCard(int cardId, IReadOnlyCollection<int> tagIds, CancellationToken token = default)
        => _tagRepository.AddTagsToCard(cardId, tagIds, token);

    public Task RemoveTagFromCard(int tagId, int cardId, CancellationToken token = default)
        => _tagRepository.RemoveTagFromCard(tagId, cardId, token);

    public Task AddTagsToCategory(int categoryId, IReadOnlyCollection<int> tagIds, CancellationToken token = default)
        => _tagRepository.AddTagsToCategory(categoryId, tagIds, token);

    public Task RemoveTagFromCategory(int tagId, int categoryId, CancellationToken token = default)
        => _tagRepository.RemoveTagFromCategory(tagId, categoryId, token);
}
