namespace Book.ViewModels.Samples.Chapter07.Sample04
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Reactive.Linq;
    using Data;
    using ReactiveUI;

    public sealed class DinosaurViewModel : ReactiveObject
    {
        private static readonly IList<Era> eras = Enum
            .GetValues(typeof(Era))
            .Cast<Era>()
            .ToList();
        private static readonly IList<Period> periods = Enum
            .GetValues(typeof(Period))
            .Cast<Period>()
            .ToList();
        private readonly string name;
        private readonly ObservableAsPropertyHelper<Color> color;
        private Era selectedEra;
        private Period selectedPeriod;

        public DinosaurViewModel(MainViewModel owner, Dinosaur model)
        {
            this.name = model.Name;
            this.selectedEra = model.Era.Value;
            this.selectedPeriod = model.Period.Value;

            // The custom DoLifetime operator allows us to track when subscriptions are made and dropped.
            // In this case, we only ever expect them to be made, never dropped. This is because we have no deactivation logic in our VM.
            this.color = this
                .WhenAnyValue(x => x.SelectedEra, x => x.SelectedPeriod, TimelineColors.GetColorFor)
                .DoLifetime(() => owner.SubscriptionCount += 1, () => owner.SubscriptionCount -= 1)
                .ToProperty(this, x => x.Color, deferSubscription: true);
        }

        public string Name => this.name;

        public IList<Era> Eras => eras;

        public IList<Period> Periods => periods;

        public Color Color => this.color.Value;

        public Era SelectedEra
        {
            get => this.selectedEra;
            set => this.RaiseAndSetIfChanged(ref this.selectedEra, value);
        }

        public Period SelectedPeriod
        {
            get => this.selectedPeriod;
            set => this.RaiseAndSetIfChanged(ref this.selectedPeriod, value);
        }
    }
}