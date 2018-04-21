namespace Book.ViewModels.Samples.Chapter10.Sample06
{
    using System;
    using Data;

    public sealed class Vote
    {
        private readonly string voter;
        private readonly Dinosaur dinosaur;
        private readonly DateTime timestamp;

        public Vote(
            string voter,
            Dinosaur dinosaur,
            DateTime? timestamp = null)
        {
            this.voter = voter;
            this.dinosaur = dinosaur;
            this.timestamp = timestamp ?? DateTime.UtcNow;
        }

        public string Voter => this.voter;

        public Dinosaur Dinosaur => this.dinosaur;

        public DateTime Timestamp => this.timestamp;
    }
}