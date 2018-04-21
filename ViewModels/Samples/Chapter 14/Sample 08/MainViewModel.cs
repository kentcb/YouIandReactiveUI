namespace Book.ViewModels.Samples.Chapter14.Sample08
{
    using System.Linq;
    using System.Reactive.Linq;
    using ReactiveUI;

    [Sample(
        "Bind: implicit converter",
        @"This sample demonstrates the use of `Bind` with implicit type converters. The `Time` property in the view model is of type `Timestamp` (a custom type), so a type converter is implicitly used in the `Bind` call to convert it to an appropriate `string`. Similarly, when the user modifies that `string`, an implicit type converter is used to convert back to `Timestamp`.")]
    public sealed class MainViewModel : ReactiveObject
    {
        private readonly ObservableAsPropertyHelper<string> display;
        private Timestamp time;

        public MainViewModel()
        {
            this.display = this
                .WhenAnyValue(x => x.Time)
                .Select(time => $"You have entered a time with {time.Ticks} ticks.")
                .ToProperty(this, x => x.Display);
        }

        public Timestamp Time
        {
            get => this.time;
            set => this.RaiseAndSetIfChanged(ref this.time, value);
        }

        public string Display => this.display.Value;
    }
}