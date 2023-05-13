namespace AnkiDoodle.Database.DbModel
{
    public class Review
    {
        public long Id {get; set;}
        public virtual User User { get; set; } = null!;
        public DateTime StartTime { get; set; }
        public Deck Deck { get; set; } = null!;
        public List<Card> Cards { get; set; } = new List<Card>();
        public List<ReviewCard> ReviewStatus { get; set; } = new List<ReviewCard>();
    }
}
