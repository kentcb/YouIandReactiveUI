namespace Book.ViewModels.Samples.Chapter10.Sample06
{
    public sealed class VoteCountViewModel
    {
        private readonly string name;
        private readonly int count;

        public VoteCountViewModel(
            string name,
            int count)
        {
            this.name = name;
            this.count = count;
        }

        public string Name => this.name;

        public int Count => this.count;

        public string Display => $"{Name} ({Count} votes)";
    }
}