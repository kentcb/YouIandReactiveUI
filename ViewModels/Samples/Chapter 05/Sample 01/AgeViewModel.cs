namespace Book.ViewModels.Samples.Chapter05.Sample01
{
    using System;
    using System.Drawing;
    using Data;

    // Notice this view model does not even extend ReactiveObject. That's because we don't need any of its features.
    // We could always extend it later if necessary.
    public sealed class AgeViewModel : IEquatable<AgeViewModel>
    {
        private readonly Era era;
        private readonly Period period;
        private readonly Color color;
        private readonly string display;

        public AgeViewModel(
            Era era,
            Period period,
            Color color,
            string display = null)
        {
            this.era = era;
            this.period = period;
            this.color = color;
            this.display = display ?? $"{this.era}, {this.period}";
        }

        public Era Era => this.era;

        public Period Period => this.period;

        public string Display => this.display;

        public Color Color => this.color;

        public bool Equals(AgeViewModel other) =>
            this.era == other.era && this.period == other.period;
    }
}