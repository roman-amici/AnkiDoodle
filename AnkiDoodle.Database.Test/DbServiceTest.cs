using AnkiDoodle.Database.DbModel;

namespace AnkiDoodle.Database.Test
{
    [TestClass]
    public class DbServiceTest
    {
        [TestMethod]
        public async Task TestAddDeck()
        {
            var deck = new Deck()
            {
                Name = "Test",
                Cards = new List<Card> {
                    new() { ContentLocation = "Content1"},
                    new() { ContentLocation = "Content2"}
                }
            };

            var service = new DbService();
            var newDeck = await service.AddDeck(deck);

            Assert.AreNotEqual(0, newDeck.Id);
            Assert.AreEqual("Test", newDeck.Name);
            Assert.AreEqual(2, newDeck.Cards.Count);

            Assert.AreEqual(2, newDeck.OrderedCards.Count);

            // Ensure the card with the earlies order is the same as the card that is first in the original list.
            var order = newDeck.OrderedCards[0];
            if (newDeck.OrderedCards[1].Order < order.Order )
            {
                order = newDeck.OrderedCards[1];
            }
            var c1 = newDeck.Cards.Find(x => x.Id == order.CardId)!;

            Assert.AreEqual(deck.Cards[0].ContentLocation, c1.ContentLocation);

        }

        [TestMethod]
        public async Task UpdateDeckDeleteCard()
        {
            var deck = new Deck()
            {
                Name = "Test",
                Cards = new List<Card> {
                    new() { ContentLocation = "Content1"},
                    new() { ContentLocation = "Content2"},
                    new() { ContentLocation = "Content3"}
                }
            };

            var service = new DbService();
            var newDeck = await service.AddDeck(deck);

            newDeck.Cards.RemoveAt(1);

            var updatedDeck = await service.UpdateDeck(newDeck);

            Assert.AreEqual(2, updatedDeck.Cards.Count);
            Assert.AreEqual(2, updatedDeck.OrderedCards.Count);
            Assert.IsNotNull(updatedDeck.Cards.Find(x => x.Id == 1));
            Assert.IsNotNull(updatedDeck.Cards.Find(x => x.Id == 3));
        }

        [TestMethod]
        public async Task UpdateDeck()
        {
            var deck = new Deck()
            {
                Name = "Test",
                Cards = new List<Card> {
                    new() { ContentLocation = "Content1"},
                    new() { ContentLocation = "Content2"}
                }
            };

            var service = new DbService();
            var newDeck = await service.AddDeck(deck);

            newDeck.Cards.Insert(1, new Card() { ContentLocation = "Content3" });

            var updatedDeck = await service.UpdateDeck(newDeck);

            var newOrder = updatedDeck.OrderedCards.Find(x => x.CardId == 3)!;

            // New card should be placed at index 1
            Assert.AreEqual(1, newOrder.Order);

            newOrder = updatedDeck.OrderedCards.Find(x => x.CardId == 1)!;
            Assert.AreEqual(0, newOrder.Order);

            newOrder = updatedDeck.OrderedCards.Find(x => x.CardId == 2)!;
            Assert.AreEqual(2, newOrder.Order);

        }
    }
}