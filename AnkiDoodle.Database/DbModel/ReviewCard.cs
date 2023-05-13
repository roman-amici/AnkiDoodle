namespace AnkiDoodle.Database.DbModel
{
    public enum ReviewMode
    {
        Learning,
        Reviewing
    }

    public class ReviewCard
    {
        public DateTime LastReview { get; set; }
        public ReviewMode ReviewMode { get; set; }
        public long ReviewStage { get; set; }
        // Ease is used after passing the review stage
        public decimal Ease { get; set; }
        public long CardId { get; set; }
        public long ReviewId { get; set; }
    }
}
