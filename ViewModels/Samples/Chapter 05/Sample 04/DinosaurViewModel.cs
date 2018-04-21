namespace Book.ViewModels.Samples.Chapter05.Sample04
{
    using Data;
    using ReactiveUI;

    public sealed class DinosaurViewModel : ReactiveObject
    {
        private string name;
        private Era era;
        private Period period;

        public string Name
        {
            get => this.name;
            set => this.RaiseAndSetIfChanged(ref this.name, value);
        }

        public Era Era
        {
            get => this.era;
            set => this.RaiseAndSetIfChanged(ref this.era, value);
        }

        public Period Period
        {
            get => this.period;
            set => this.RaiseAndSetIfChanged(ref this.period, value);
        }
    }
}