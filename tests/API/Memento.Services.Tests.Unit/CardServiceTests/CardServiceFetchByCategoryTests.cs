using System.Threading.Tasks;
using FakeItEasy;
using Memento.Infrastructure.Entities;
using Memento.Infrastructure.Repositories;
using Memento.Services.Services;
using Xunit;

namespace Memento.Services.Tests.Unit.CardServiceTests;

public sealed class CardServiceGetAllCardsTests
{
    [Fact]
    public async Task Should_map_and_return_cards_when_found()
    {
        // Arrange
        var cardEntity = new CardEntity
        {
            Id = 1,
            Word = "Word",
            Translation = "Translation",
            Definition = "Definition",
            Hint = "Hint",
            Image = "Image",
        };

        var repository = A.Fake<ICardRepository>();
        A.CallTo(() => repository.GetAllCards()).Returns([cardEntity]);

        var sevice = new CardService(repository);

        // Act
        var cards = await sevice.GetAllCards();

        // Assert
        var card = Assert.Single(cards);
        Assert.Equal(cardEntity.Id, card.Id);
        Assert.Equal(cardEntity.Word, card.Word);
        Assert.Equal(cardEntity.Translation, card.Translation);
        Assert.Equal(cardEntity.Definition, card.Definition);
        Assert.Equal(cardEntity.Hint, card.Hint);
        Assert.Equal(cardEntity.Image, card.Image);
    }
    
    [Fact]
    public async Task Should_return_empty_collection_when_not_found()
    {
        // Arrange
        var repository = A.Fake<ICardRepository>();
        A.CallTo(() => repository.GetAllCards()).Returns([]);

        var sevice = new CardService(repository);

        // Act
        var cards = await sevice.GetAllCards();

        // Assert
        Assert.Empty(cards);
    }
}
