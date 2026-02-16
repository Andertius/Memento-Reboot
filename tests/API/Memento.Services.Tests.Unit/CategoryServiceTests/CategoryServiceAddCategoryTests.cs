using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using Memento.Infrastructure.Entities;
using Memento.Infrastructure.Repositories;
using Memento.Services.Models;
using Memento.Services.Services;
using Xunit;

namespace Memento.Services.Tests.Unit.CategoryServiceTests;

public sealed class CategoryServiceAddCategoryTests
{
    [Fact]
    public async Task Should_add_category_when_does_not_exist()
    {
        // Arrange
        var category = new Category
        {
            Name = "Name",
            Description = "Description",
            Image = "Image",
        };

        var repository = A.Fake<ICategoryRepository>();
        A.CallTo(() => repository.GetByName(A<string>.Ignored)).Returns<CategoryEntity?>(null);
        var service = new CategoryService(repository);
    
        // Act
        await service.AddCategory(category, CancellationToken.None);

        // Assert
        var expression = GetPredicate(category);
        A.CallTo(() => repository.AddCategory(A<CategoryEntity>.That.Matches(expression))).MustHaveHappenedOnceExactly();

        return;

        static Expression<Func<CategoryEntity, bool>> GetPredicate(Category category)
            => entity => category.Name == entity.Name &&
                         category.Description == entity.Description &&
                         category.Image == entity.Image;
    }

    [Fact]
    public async Task Should_return_when_category_exists()
    {
        // Arrange
        var category = new Category
        {
            Name = "Name",
            Description = "Description",
            Image = "Image",
        };

        var repository = A.Fake<ICategoryRepository>();
        A.CallTo(() => repository.GetByName(A<string>.Ignored)).Returns(new CategoryEntity());
        var service = new CategoryService(repository);
    
        // Act
        await service.AddCategory(category, CancellationToken.None);

        // Assert
        var expression = GetPredicate(category);
        A.CallTo(() => repository.AddCategory(A<CategoryEntity>.That.Matches(expression))).MustNotHaveHappened();

        return;

        static Expression<Func<CategoryEntity, bool>> GetPredicate(Category category)
            => entity => category.Name == entity.Name &&
                         category.Description == entity.Description &&
                         category.Image == entity.Image;
    }
}
