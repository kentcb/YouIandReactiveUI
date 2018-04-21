namespace Book.ViewModels.Samples.Chapter14.Sample07
{
    using System.Linq;
    using System.Reactive.Linq;
    using ReactiveUI;

    [Sample(
        "Bind: selectors",
        @"This sample demonstrates the use of selectors with `Bind`, enabling values to be converted before propagation in either direction. The `Time` property on the view model is of type `Timestamp?`, which the view must convert it to a `string`. Similarly, the user enters a `Timestamp` by typing in a `string`, so it must be converted back to a `Timestamp`.")]
    public sealed class MainViewModel : ReactiveObject
    {
        private readonly ObservableAsPropertyHelper<string> display;
        private Timestamp? time;

        public MainViewModel()
        {
            this.display = this
                .WhenAnyValue(x => x.Time)
                .Select(time => time == null ? "Please enter a valid time in the format 'HH:mm:ss'." : $"You have entered a time with {time.Value.Ticks} ticks.")
                .ToProperty(this, x => x.Display);
        }

        public Timestamp? Time
        {
            get => this.time;
            set => this.RaiseAndSetIfChanged(ref this.time, value);
        }

        public string Display => this.display.Value;
    }
}