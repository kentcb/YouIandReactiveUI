namespace Book.ViewModels.Samples.Chapter05.Sample01
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive.Linq;
    using System.Text;
    using global::ReactiveUI;

    [Sample(
        "Change Notifications",
        @"This sample demonstrates change notifications.

Changing the name, weight, or age of the dinosaur will result in two notifications: one before the change, and one after. These are made available via the `Changing` and `Changed` observables exposed by `ReactiveObject`.

Notice that the first two ages in the list have the same era and period. The backing view model for the age overrides equality checks so that selecting one of these ages and then the other will *not* result in a change notification.")]
    public sealed class MainViewModel : ReactiveObject
    {
        private readonly List<AgeViewModel> ages;
        private readonly DinosaurViewModel dinosaur;
        private readonly ObservableAsPropertyHelper<string> output;

        public MainViewModel()
        {
            this.ages = new[]
                {
                    // purposely put a duplicate at the start of our list so we can see how it behaves under RaiseAndSetIfChanged
                    new AgeViewModel(Data.Era.Palaeozoic, Data.Period.Cambrian, Data.TimelineColors.GetColorFor(Data.Era.Palaeozoic, Data.Period.Cambrian), "Duplicate of the below (Palaeozoic, Cambrian)")
                }.Concat(
                    Data
                        .TimelineColors
                        .GetColors()
                        .Select(colorInfo => new AgeViewModel(colorInfo.era, colorInfo.period, colorInfo.color)))
                .ToList();
            this.dinosaur = new DinosaurViewModel();

            var changing = this
                .dinosaur
                .Changing
                .Select(e => $"Property '{e.PropertyName}' changing.");
            var changed = this
                .dinosaur
                .Changed
                .Select(e => $"Property '{e.PropertyName}' changed.");
            var messages = Observable
                .Merge(
                    changing,
                    changed);

            this.output = messages
                .Scan(new StringBuilder(), (sb, next) => sb.AppendLine(next))
                .Select(x => x.ToString())
                .ToProperty(this, x => x.Output);
        }

        public IList<AgeViewModel> Ages => this.ages;

        public DinosaurViewModel Dinosaur => this.dinosaur;

        public string Output => this.output.Value;
    }
}