using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Memento.Infrastructure.Repositories;
using Memento.Services.Mappers;
using Memento.Services.Models;

namespace Memento.Services.Services;

public interface ITagService
{
    Task<Tag[]> GetAllTags();
    Task<Tag?> GetTagById(int id);
    Task<int> AddTag(Tag tag);
    Task RemoveTag(int id);
    Task AddTagsToCard(int cardId, IReadOnlyCollection<int> tagIds);
    Task RemoveTagFromCard(int tagId, int cardId);
    Task AddTagsToCategory(int categoryId, IReadOnlyCollection<int> tagIds);
    Task RemoveTagFromCategory(int tagId, int categoryId);
}

public sealed class TagService(ITagRepository tagRepository) : ITagService
{
    private readonly ITagRepository _tagRepository = tagRepository ?? throw new ArgumentNullException(nameof(tagRepository), "Tag Repository must not be null");
    private readonly TagMapper _tagMapper = new();

    public async Task<Tag[]> GetAllTags()
    {
        var cards = await _tagRepository.GetAllTags();
        return cards.Select(_tagMapper.MapTagEntityToTag).ToArray();
    }

    public async Task<Tag?> GetTagById(int id)
    {
        var entity = await _tagRepository.GetById(id);

        return entity is null
            ? null
            : _tagMapper.MapTagEntityToTag(entity);
    }

    public async Task<int> AddTag(Tag tag)
    {
        var tagEntity = _tagMapper.MapTagToTagEntity(tag);
        return await _tagRepository.AddTag(tagEntity);
    }

    public Task RemoveTag(int id)
        => _tagRepository.RemoveTag(id);

    public Task AddTagsToCard(int cardId, IReadOnlyCollection<int> tagIds)
        => _tagRepository.AddTagsToCard(cardId, tagIds);

    public Task RemoveTagFromCard(int tagId, int cardId)
        => _tagRepository.RemoveTagFromCard(tagId, cardId);

    public Task AddTagsToCategory(int categoryId, IReadOnlyCollection<int> tagIds)
        => _tagRepository.AddTagsToCategory(categoryId, tagIds);

    public Task RemoveTagFromCategory(int tagId, int categoryId)
        => _tagRepository.RemoveTagFromCategory(tagId, categoryId);
}
