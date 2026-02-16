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

namespace Memento.Services.Tests.Unit.CardServiceTests;

public sealed class CardServiceAddCardTests
{
    [Fact]
    public async Task Should_add_mapped_card_if_does_not_exist()
    {
        // Arrange
        var card = new Card
        {
            Word = "Word",
            Translation = "Translation",
            Definition = "Definition",
            Hint = "Hint",
            Image = "Image",
        };

        var repository = A.Fake<ICardRepository>();
        var service = new CardService(repository);
        
        // Act
        await service.AddCard(card, CancellationToken.None);

        // Assert
        var expression = GetPredicate(card);
        A.CallTo(() => repository.AddCard(A<CardEntity>.That.Matches(expression))).MustHaveHappenedOnceExactly();

        return;

        static Expression<Func<CardEntity, bool>> GetPredicate(Card card)
            => entity => card.Word == entity.Word &&
                         card.Translation == entity.Translation &&
                         card.Definition == entity.Definition &&
                         card.Hint == entity.Hint &&
                         card.Image == entity.Image;
    }
}
