using System.Threading.Tasks;
using FakeItEasy;
using Memento.Infrastructure.Entities;
using Memento.Infrastructure.Repositories;
using Memento.Services.Services;
using Xunit;

namespace Memento.Services.Tests.Unit.CategoryServiceTests;

public sealed class CategoryServiceGetByNameTests
{
    [Fact]
    public async Task Should_return_category_when_found()
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
        A.CallTo(() => repository.GetByName("Name")).Returns(categoryEntity);

        var sevice = new CategoryService(repository);

        // Act
        var category = await sevice.GetByName("Name");

        // Assert
        Assert.NotNull(category);
        Assert.Equal(categoryEntity.Id, category.Id);
        Assert.Equal(categoryEntity.Name, category.Name);
        Assert.Equal(categoryEntity.Description, category.Description);
        Assert.Equal(categoryEntity.Image, category.Image);
    }

    [Fact]
    public async Task Should_return_null_when_not_found()
    {
        // Arrange
        var repository = A.Fake<ICategoryRepository>();
        A.CallTo(() => repository.GetByName("Name")).Returns<CategoryEntity?>(null);

        var sevice = new CategoryService(repository);

        // Act
        var category = await sevice.GetByName("Name");

        // Assert
        Assert.Null(category);
    }
}
