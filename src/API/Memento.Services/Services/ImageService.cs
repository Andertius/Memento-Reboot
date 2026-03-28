using System;
using System.IO;
using System.IO.Abstractions;
using System.Threading;
using System.Threading.Tasks;
using Memento.Infrastructure.Repositories;
using Memento.Services.Interfaces;

namespace Memento.Services.Services;

public interface IImageService
{
    Task<string?> UploadCardImageAsync(Stream image, string fileName, int cardId, CancellationToken token = default);

    Task DeleteCardImageAsync(int cardId, CancellationToken token = default);

    Task<string?> UploadCategoryImageAsync(Stream image, string fileName, int categoryId, CancellationToken token = default);

    Task DeleteCategoryImageAsync(int categoryId,  CancellationToken token = default);
}

public sealed class ImageService(
    IFileSystem _fileSystem,
    ICardRepository _cardRepository,
    ICategoryRepository _categoryRepository,
    WebRootPathProvider _webRootPathProvider) : IImageService
{
    public async Task<string?> UploadCardImageAsync(Stream image, string fileName, int cardId, CancellationToken token = default)
    {
        (bool exists, string? previousFileName) = await _cardRepository.GetImageName(cardId, token);
        
        Console.WriteLine(exists);

        if (!exists)
        {
            return null;
        }

        if (!String.IsNullOrWhiteSpace(previousFileName))
        {
            string previousFilePath = GetCardsPath(previousFileName);
            DeleteFileIfExists(previousFilePath);
        }

        string guidFileName = GenerateFileName(fileName);
        string filePath = GetCardsPath(guidFileName);

        await using var fileStream = _fileSystem.File.OpenWrite(filePath);
        await image.CopyToAsync(fileStream, token);
        await _cardRepository.UpsertImage(cardId, guidFileName, token);

        return guidFileName;
    }

    public async Task DeleteCardImageAsync(int cardId, CancellationToken token = default)
    {
        (_, string? fileName) = await _cardRepository.GetImageName(cardId, token);

        if (String.IsNullOrWhiteSpace(fileName))
        {
            return;
        }

        string filePath = GetCardsPath(fileName);
        
        DeleteFileIfExists(filePath);
        await _cardRepository.RemoveImage(cardId, token);
    }

    public async Task<string?> UploadCategoryImageAsync(Stream image, string fileName, int categoryId, CancellationToken token = default)
    {
        (bool exists, string? previousFileName) = await _categoryRepository.GetImageName(categoryId, token);

        if (!exists)
        {
            return null;
        }

        if (!String.IsNullOrWhiteSpace(previousFileName))
        {
            string previousFilePath = GetCategoriesPath(previousFileName);
            DeleteFileIfExists(previousFilePath);
        }

        string guidFileName = GenerateFileName(fileName);
        string filePath = GetCategoriesPath(guidFileName);

        await using var fileStream = _fileSystem.File.OpenWrite(filePath);
        await image.CopyToAsync(fileStream, token);
        await _categoryRepository.UpsertImage(categoryId, guidFileName, token);

        return guidFileName;
    }

    public async Task DeleteCategoryImageAsync(int categoryId, CancellationToken token = default)
    {
        (_, string? fileName) = await _categoryRepository.GetImageName(categoryId, token);

        if (String.IsNullOrWhiteSpace(fileName))
        {
            return;
        }

        string filePath = GetCategoriesPath(fileName);

        DeleteFileIfExists(filePath);
        await _categoryRepository.RemoveImage(categoryId, token);
    }

    private string GenerateFileName(string fileName)
    {
        string extension = _fileSystem.Path.GetExtension(fileName);

        return Guid.NewGuid() + extension;
    }

    private string GetCardsPath(string fileName)
        => _fileSystem.Path.Combine(_webRootPathProvider.ImageRootPath, "cards", fileName);

    private string GetCategoriesPath(string fileName)
        => _fileSystem.Path.Combine(_webRootPathProvider.ImageRootPath, "categories", fileName);

    private void DeleteFileIfExists(string filePath)
    {
        if (_fileSystem.File.Exists(filePath))
        {
            _fileSystem.File.Delete(filePath);
        }
    }
}
