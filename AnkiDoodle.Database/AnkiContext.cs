using AnkiDoodle.Database.DbModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnkiDoodle.Database
{
    public class AnkiContext : DbContext
    {
        public DbSet<Card> Cards { get; set; }
        public DbSet<Deck> Decks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<ReviewCard> ReviewCards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Deck>()
                .HasMany(e => e.Cards)
                .WithMany(e => e.Decks)
                .UsingEntity<CardOrder>();

            modelBuilder.Entity<Review>()
                .HasMany(e => e.Cards)
                .WithMany(e => e.Reviews)
                .UsingEntity<ReviewCard>();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseSqlite("Data Source=anki.db");
        }
    }
}
