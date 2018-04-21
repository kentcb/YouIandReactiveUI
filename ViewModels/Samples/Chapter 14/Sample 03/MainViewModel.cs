namespace Book.ViewModels.Samples.Chapter14.Sample03
{
    using System;
    using System.Linq;
    using System.Reactive.Linq;
    using ReactiveUI;

    [Sample(
        "OneWayBind: implicit converter",
        @"This sample demonstrates the use of `OneWayBind` with an implicit type converter. The `Time` property in the view model is of type `Timestamp` (a custom type), so a type converter is implicitly used in the `OneWayBind` call to convert it to an appropriate `string`.")]
    public sealed class MainViewModel : ReactiveObject
    {
        private readonly ObservableAsPropertyHelper<Timestamp> time;

        public MainViewModel()
        {
            this.time = Observable
                .Timer(TimeSpan.Zero, TimeSpan.FromSeconds(1), RxApp.MainThreadScheduler)
                .Select(_ => new Timestamp(DateTime.Now.Ticks))
                .ToProperty(this, x => x.Time);
        }

        public Timestamp Time => this.time.Value;
    }
}