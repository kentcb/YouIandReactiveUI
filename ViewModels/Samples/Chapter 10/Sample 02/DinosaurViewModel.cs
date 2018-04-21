namespace Book.ViewModels.Samples.Chapter10.Sample02
{
    using ReactiveUI;

    public sealed class DinosaurViewModel : ReactiveObject
    {
        private readonly string name;
        private int? fossilCount;

        public DinosaurViewModel(
            string name,
            int fossilCount)
        {
            this.name = name;
            this.fossilCount = fossilCount;
        }

        public string Name => this.name;

        public int? FossilCount
        {
            get => this.fossilCount;
            set => this.RaiseAndSetIfChanged(ref this.fossilCount, value);
        }
    }
}