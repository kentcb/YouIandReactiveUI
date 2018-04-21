namespace Book.ViewModels.Samples.Chapter06.Sample05
{
    using System;
    using System.Reactive.Linq;
    using ReactiveUI;

    public sealed class ExhibitionViewModel : ReactiveObject
    {
        private readonly string name;
        private readonly IObservable<TimeSpan> openCountdown;

        public ExhibitionViewModel(
            string name,
            DateTime openTime)
        {
            this.name = name;
            this.openCountdown = Observable
                .Defer(
                    () =>
                        Observable
                            .Timer(TimeSpan.Zero, TimeSpan.FromSeconds(1), RxApp.MainThreadScheduler)
                            .TimeInterval()
                            .Scan(openTime - DateTime.Now, (acc, next) => acc.Subtract(next.Interval)));
        }

        public string Name => this.name;

        // Importantly, we're exposing the countdown as an observable here, not as a scalar property of type TimeSpan
        // Admittedly, this is concocted because normally view models would convert the observable to a scalar property
        public IObservable<TimeSpan> OpenCountdown => this.openCountdown;
    }
}