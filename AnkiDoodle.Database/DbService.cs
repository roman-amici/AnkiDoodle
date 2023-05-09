using AnkiDoodle.Database.DbModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnkiDoodle.Database
{
    public class DbService
    {
        private readonly AnkiContext _context;

        public DbService()
        {
            _context = new();

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }

        public async Task TestAdd()
        {
            var deck = new Deck() { Name = "Test Deck" };
            _context.Add(deck);
            await _context.SaveChangesAsync();

            var card1 = new Card() { ContentLocation = "Test1" };
            var card2 = new Card() { ContentLocation = "Test2" };
            var card3 = new Card() { ContentLocation = "Test3" };

            _context.Add(card1);
            _context.Add(card2);
            _context.Add(card3);

            await _context.SaveChangesAsync();

            var cardOrder1 = new CardOrder() { CardId = card1.Id, DeckId = deck.Id, Order = 3 };
            var cardOrder2 = new CardOrder() { CardId = card2.Id, DeckId = deck.Id, Order = 2 };
            var cardOrder3 = new CardOrder() { CardId = card3.Id, DeckId = deck.Id, Order = 1 };

            _context.Add(cardOrder1);
            _context.Add(cardOrder2);
            _context.Add(cardOrder3);

            await _context.SaveChangesAsync();

            Console.WriteLine(deck.Cards.Count);

        }


    }
}
