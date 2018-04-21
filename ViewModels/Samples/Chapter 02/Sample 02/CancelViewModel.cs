namespace Book.ViewModels.Samples.Chapter02.Sample02
{
    using System;
    using System.Linq;
    using System.Reactive.Linq;
    using ReactiveUI;

    public sealed class CancelViewModel : ReactiveObject
    {
        private const int timeAllowed = 5;
        private readonly ObservableAsPropertyHelper<int> timeRemaining;

        public CancelViewModel()
        {
            this.timeRemaining = Observable
                .Timer(TimeSpan.Zero, TimeSpan.FromSeconds(1), RxApp.MainThreadScheduler)
                .Scan(timeAllowed, (acc, _) => --acc)
                .Take(timeAllowed)
                .StartWith(timeAllowed)
                .ToProperty(this, x => x.TimeRemaining);
        }

        public int TimeRemaining => this.timeRemaining.Value;
    }
}