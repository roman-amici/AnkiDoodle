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

        public DbService(AnkiContext context)
        {
            _context = context;
        }

        public DbService()
        {
            _context = new();

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }

        public async Task<Deck> AddDeck(Deck deck)
        {
            if (deck.Id != 0)
            {
                throw new ArgumentException("Cannot add a deck which has already been added.");
            }

            using var txn = _context.Database.BeginTransaction();

            var newDeck = new Deck() { Name = deck.Name };
            await _context.Decks.AddAsync(newDeck);

            foreach (var card in deck.Cards)
            {
                if (card.Id == 0) 
                {
                    await _context.Cards.AddAsync(card);
                }
            }

            await _context.SaveChangesAsync();

            foreach (var (card, order) in deck.Cards.Select((c,i) => (c,i)))
            {
                _context.Add(new CardOrder() { CardId = card.Id, Order = order, DeckId = newDeck.Id });
            }

            await _context.SaveChangesAsync();

            await txn.CommitAsync();

            return newDeck;
        }

        public async Task<Deck> UpdateDeck(Deck deck)
        {
            if (deck.Id == 0)
            {
                throw new ArgumentException("Cannot update a deck which has not first been added.");
            }

            using var txn = _context.Database.BeginTransaction();

            var currentDeck = await _context.Decks.FindAsync(deck.Id);
            if (currentDeck == null)
            {
                throw new ArgumentException($"Deck with id {deck.Id} not found");
            }

            foreach (var card in deck.Cards.Where(x => x.Id == 0))
            {
                await _context.Cards.AddAsync(card);
            }

            foreach (var card in currentDeck.Cards.Where(x => x.Id != 0))
            {
                if (deck.Cards.Find(x => x.Id == card.Id) == null)
                {
                    _context.Cards.Remove(card);
                }
            }

            // Add Id's to cards.
            await _context.SaveChangesAsync();

            // Update the current order to match the order of the deck
            foreach (var (card, order) in deck.Cards.Select((c,i) => (c,i)))
            {
                var existingOrder = currentDeck.OrderedCards.Find(x => x.CardId == card.Id);
                if (existingOrder == null)
                {
                    await _context.AddAsync( new CardOrder() { CardId = card.Id, Order = order, DeckId = deck.Id });
                }
                else
                {
                    existingOrder.Order = order;
                }
            }

            await _context.SaveChangesAsync();

            await txn.CommitAsync();

            return currentDeck;
        }
    }
}
