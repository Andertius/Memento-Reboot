using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using Memento.Infrastructure.Entities;
using Memento.Infrastructure.Repositories;
using Memento.Services.Services;
using Xunit;

namespace Memento.Services.Tests.Unit.CategoryServiceTests;

public sealed class CategoryServiceGetAllCategoriesTests
{
    [Fact]
    public async Task Should_map_and_return_categories_when_found()
    {
        // Arrange
        var categoryEntity = new CategoryEntity
        {
            Id = 1,
            Name = "Name",
            Description = "Description",
            Image = "Image",
        };

        var repository = A.Fake<ICategoryRepository>();
        A.CallTo(() => repository.GetAllCategories()).Returns([categoryEntity]);

        var sevice = new CategoryService(repository);

        // Act
        var categories = await sevice.GetAllCategories(CancellationToken.None);

        // Assert
        var category = Assert.Single(categories);
        Assert.Equal(categoryEntity.Id, category.Id);
        Assert.Equal(categoryEntity.Name, category.Name);
        Assert.Equal(categoryEntity.Description, category.Description);
        Assert.Equal(categoryEntity.Image, category.Image);
    }

    [Fact]
    public async Task Should_return_empty_collection_when_not_found()
    {
        // Arrange
        var repository = A.Fake<ICategoryRepository>();
        A.CallTo(() => repository.GetAllCategories()).Returns([]);

        var sevice = new CategoryService(repository);

        // Act
        var cartegories = await sevice.GetAllCategories(CancellationToken.None);

        // Assert
        Assert.Empty(cartegories);
    }
}
