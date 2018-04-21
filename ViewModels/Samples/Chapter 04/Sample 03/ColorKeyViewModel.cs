namespace Book.ViewModels.Samples.Chapter04.Sample03
{
    using System.Drawing;
    using Data;
    using ReactiveUI;

    public sealed class ColorKeyViewModel : ReactiveObject
    {
        private readonly Era era;
        private readonly Period period;
        private readonly Color color;

        public ColorKeyViewModel(
            Era era,
            Period period,
            Color color)
        {
            this.era = era;
            this.period = period;
            this.color = color;
        }

        public Era Era => this.era;

        public Period Period => this.period;

        public Color Color => this.color;
    }
}