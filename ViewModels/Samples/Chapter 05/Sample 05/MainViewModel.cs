namespace Book.ViewModels.Samples.Chapter05.Sample05
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive.Linq;
    using System.Text;
    using ReactiveUI;

    [Sample(
        "Change Notifications via IReactiveObject",
        @"This sample exhibits the same behavior as [sample 05.01](05.01), but does so by implementing `IReactiveObject` rather than extending `ReactiveObject`.")]
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

            // ReactiveObject provides Changing and Changed observables, which we don't have now that our DinosaurViewModel implements IReactiveObject
            var changing = Observable
                .FromEventPattern<PropertyChangingEventHandler, PropertyChangingEventArgs>(x => this.dinosaur.PropertyChanging += x, x => this.dinosaur.PropertyChanging -= x)
                .Select(e => $"Property '{e.EventArgs.PropertyName}' changing.");
            var changed = Observable
                .FromEventPattern<System.ComponentModel.PropertyChangedEventHandler, System.ComponentModel.PropertyChangedEventArgs>(x => this.dinosaur.PropertyChanged += x, x => this.dinosaur.PropertyChanged -= x)
                .Select(e => $"Property '{e.EventArgs.PropertyName}' changed.");
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