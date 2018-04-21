namespace Book.ViewModels.Samples.Chapter15.Sample01
{
    using System;
    using System.Linq;
    using System.Reactive.Linq;
    using ReactiveUI;

    public sealed class CountdownViewModel : ReactiveObject
    {
        private readonly ObservableAsPropertyHelper<TimeSpan> timeRemaining;

        public CountdownViewModel()
        {
            this.timeRemaining = Observable
                .Timer(TimeSpan.Zero, TimeSpan.FromSeconds(1), RxApp.MainThreadScheduler)
                .Scan(TimeSpan.FromMinutes(1), (acc, _) => acc.Add(TimeSpan.FromSeconds(-1)))
                .ToProperty(this, x => x.TimeRemaining, deferSubscription: true);
        }

        public TimeSpan TimeRemaining => this.timeRemaining.Value;
    }
}