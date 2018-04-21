namespace Book.ViewModels.Samples.Chapter14.Sample01
{
    using System;
    using System.Linq;
    using System.Reactive.Linq;
    using ReactiveUI;

    [Sample(
        "OneWayBind: simple",
        @"This sample demonstrates the use of `OneWayBind` by binding a `Time` property in the view model to an appropriate control in the view. As the `Time` property changes (every second), the view automatically reflects that change.")]
    public sealed class MainViewModel : ReactiveObject
    {
        private readonly ObservableAsPropertyHelper<string> time;

        public MainViewModel()
        {
            this.time = Observable
                .Timer(
                    TimeSpan.Zero,
                    TimeSpan.FromSeconds(1),
                    RxApp.MainThreadScheduler)
                .Select(_ => DateTime.Now.ToString("HH:mm:ss"))
                .ToProperty(this, x => x.Time);
        }

        public string Time => this.time.Value;
    }
}