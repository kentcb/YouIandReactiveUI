namespace Book.ViewModels.Samples.Chapter07.Sample01
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Reactive.Linq;
    using Data;
    using ReactiveUI;

    [Sample(
        "ToProperty",
        @"This sample demonstrates the use of `ToProperty` to create one property that is based on another.

The `Color` property is based on the chosen `Era`.")]
    public sealed class MainViewModel : ReactiveObject
    {
        private readonly IList<Era> eras;
        private readonly ObservableAsPropertyHelper<Color> color;
        private Era selectedEra;

        public MainViewModel()
        {
            this.eras = Enum
                .GetValues(typeof(Era))
                .Cast<Era>()
                .ToList();

            this.color = this
                .WhenAnyValue(x => x.SelectedEra)
                .Select(TimelineColors.GetColorFor)
                .ToProperty(this, x => x.Color);
        }

        public IList<Era> Eras => this.eras;

        public Color Color => this.color.Value;

        public Era SelectedEra
        {
            get => this.selectedEra;
            set => this.RaiseAndSetIfChanged(ref this.selectedEra, value);
        }
    }
}