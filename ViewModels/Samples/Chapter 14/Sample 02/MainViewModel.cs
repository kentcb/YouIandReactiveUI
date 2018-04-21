namespace Book.ViewModels.Samples.Chapter14.Sample02
{
    using System;
    using System.Linq;
    using System.Reactive.Linq;
    using ReactiveUI;

    [Sample(
        "OneWayBind: selector",
        @"This sample demonstrates the use of `OneWayBind` with a selector. The `Time` property in the view model is of type `DateTime`, so a selector is used in the `OneWayBind` call to convert it to an appropriate `string`.")]
    public sealed class MainViewModel : ReactiveObject
    {
        private readonly ObservableAsPropertyHelper<DateTime> time;

        public MainViewModel()
        {
            this.time = Observable
                .Timer(TimeSpan.Zero, TimeSpan.FromSeconds(1), RxApp.MainThreadScheduler)
                .Select(_ => DateTime.Now)
                .ToProperty(this, x => x.Time);
        }

        public DateTime Time => this.time.Value;
    }
}