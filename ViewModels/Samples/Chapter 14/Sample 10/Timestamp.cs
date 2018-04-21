namespace Book.ViewModels.Samples.Chapter14.Sample10
{
    public struct Timestamp
    {
        private readonly long ticks;

        public Timestamp(long ticks)
        {
            this.ticks = ticks;
        }

        public long Ticks => this.ticks;
    }
}