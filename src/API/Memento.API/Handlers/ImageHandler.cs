using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Memento.Infrastructure.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Memento.API.Handlers;

public interface IImageHandler
{
    Task<string?> UploadCardImageAsync(IFormFile image, int cardId, CancellationToken token = default);

    Task DeleteCardImageAsync(int cardId, CancellationToken token = default);

    Task<string?> UploadCategoryImageAsync(IFormFile image, int categoryId, CancellationToken token = default);

    Task DeleteCategoryImageAsync(int categoryId,  CancellationToken token = default);
}

public sealed class ImageHandler(
    IWebHostEnvironment _webHostEnvironment,
    ICardRepository _cardRepository,
    ICategoryRepository _categoryRepository) : IImageHandler
{
    public async Task<string?> UploadCardImageAsync(IFormFile image, int cardId, CancellationToken token = default)
    {
        (bool exists, string? previousFileName) = await _cardRepository.GetImageName(cardId, token);

        if (!exists)
        {
            return null;
        }

        if (!String.IsNullOrWhiteSpace(previousFileName))
        {
            string previousFilePath = GetCardsPath(previousFileName);
            DeleteFileIfExists(previousFilePath);
        }

        string fileName = GenerateFileName(image.FileName);
        string filePath = GetCardsPath(fileName);

        await using var stream = new FileStream(filePath, FileMode.OpenOrCreate);
        await image.CopyToAsync(stream, token);
        await _cardRepository.UpsertImage(cardId, fileName, token);

        return fileName;
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

    public async Task<string?> UploadCategoryImageAsync(IFormFile image, int categoryId, CancellationToken token = default)
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

        string fileName = GenerateFileName(image.FileName);
        string filePath = GetCategoriesPath(fileName);

        await using var stream = new FileStream(filePath, FileMode.OpenOrCreate);
        await image.CopyToAsync(stream, token);
        await _categoryRepository.UpsertImage(categoryId, fileName, token);

        return fileName;
    }

    public async Task DeleteCategoryImageAsync(int categoryId,  CancellationToken token = default)
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

    private static string GenerateFileName(string fileName)
    {
        string extension = Path.GetExtension(fileName);

        return Guid.NewGuid() + extension;
    }

    private string GetCardsPath(string fileName)
        => Path.Combine(_webHostEnvironment.WebRootPath, "images", "cards", fileName);

    private string GetCategoriesPath(string fileName)
        => Path.Combine(_webHostEnvironment.WebRootPath, "images", "categories", fileName);

    private static void DeleteFileIfExists(string filePath)
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }
}
