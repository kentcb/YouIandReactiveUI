namespace Book.ViewModels.Samples.Chapter04.Sample03
{
    using System.Drawing;
    using Data;
    using ReactiveUI;

    public sealed class DinosaurViewModel : ReactiveObject
    {
        private readonly string name;
        private readonly Era era;
        private readonly Period period;
        private readonly string periodDisplay;
        private readonly Color color;

        public DinosaurViewModel(Dinosaur model)
        {
            this.name = model.Name;
            this.era = model.Era.Value;
            this.period = model.Period.Value;
            this.periodDisplay = $"{this.era}, {this.period}";
            this.color = TimelineColors.GetColorFor(this.era, this.period);
        }

        public string Name => this.name;

        public Era Era => this.era;

        public Period Period => this.period;

        public string PeriodDisplay => this.periodDisplay;

        public Color Color => this.color;
    }
}