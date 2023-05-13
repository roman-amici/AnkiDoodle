using AnkiDoodle.Database.DbModel;
using Microsoft.EntityFrameworkCore;
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

        public async Task<User> CreateUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<(Review, List<ReviewCard>)> StartReview(long deckId)
        {
            var deck = await _context.Decks.FindAsync( deckId );
            if (deck == null)
            {
                throw new ArgumentException($"Could not find deck with id ${deckId}");
            }

            using var txn = await _context.Database.BeginTransactionAsync();

            var review = new Review();
            review.Deck = deck;
            _context.Add(review);

            await _context.SaveChangesAsync();

            var reviewCards = new List<ReviewCard>();
            foreach(var card in deck.Cards)
            {

                var reviewCard = new ReviewCard()
                {
                    CardId = card.Id,
                    ReviewId = review.Id,
                    Ease = 0,
                    LastReview = DateTime.UtcNow,
                    ReviewMode = ReviewMode.Learning,
                    ReviewStage = 0
                };
                _context.Add(reviewCard);
            }

            await _context.SaveChangesAsync();

            await txn.CommitAsync();

            return (review, reviewCards);
        }

        public async Task UpdateReview(IList<ReviewCard> reviewCards)
        {
            using var txn = await _context.Database.BeginTransactionAsync();

            // TODO: Make better somehow...
            foreach (var rc in reviewCards)
            {
                var dbRc = _context.ReviewCards
                    .Where(x => x.CardId == rc.CardId && x.ReviewId == rc.ReviewId)
                    .FirstOrDefault();
                if (dbRc != null)
                {
                    dbRc.Ease = rc.Ease;
                    dbRc.LastReview = rc.LastReview;
                    dbRc.ReviewMode = rc.ReviewMode;
                    dbRc.ReviewStage = rc.ReviewStage;
                }
            }

            await _context.SaveChangesAsync();

            await txn.CommitAsync();
        }

        public async Task<(Review, List<ReviewCard>)> LookupReview(long reviewId)
        {
            var review = await _context.Reviews.FindAsync(reviewId);

            if (review == null)
            {
                throw new ArgumentException($"Review with id {reviewId} was not found");
            }

            var reviewCards = _context.ReviewCards.Where(x => x.ReviewId == reviewId).ToList();

            return (review, reviewCards);

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
