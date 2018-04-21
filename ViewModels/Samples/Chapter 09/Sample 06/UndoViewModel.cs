namespace Book.ViewModels.Samples.Chapter09.Sample06
{
    using System;
    using System.Linq;
    using System.Reactive.Linq;
    using ReactiveUI;

    public sealed class UndoViewModel : ReactiveObject
    {
        private const int timeAllowed = 5;
        private readonly string name;
        private readonly ObservableAsPropertyHelper<int> timeRemaining;

        public UndoViewModel(string name)
        {
            this.name = name;

            this.timeRemaining = Observable
                .Timer(TimeSpan.Zero, TimeSpan.FromSeconds(1), RxApp.MainThreadScheduler)
                .Scan(timeAllowed, (acc, _) => --acc)
                .Take(timeAllowed)
                .StartWith(timeAllowed)
                .ToProperty(this, x => x.TimeRemaining);
        }

        public string Name => this.name;

        public int TimeRemaining => this.timeRemaining.Value;
    }
}