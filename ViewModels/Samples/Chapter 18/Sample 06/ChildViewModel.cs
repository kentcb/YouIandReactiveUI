namespace Book.ViewModels.Samples.Chapter18.Sample06
{
    using System;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;
    using ReactiveUI;

    public sealed class ChildViewModel : ReactiveObject, ISupportsActivation
    {
        private readonly ViewModelActivator activator;
        private readonly Subject<string> messages;
        private readonly ObservableAsPropertyHelper<int> timeActivated;

        public ChildViewModel()
        {
            this.activator = new ViewModelActivator();
            this.messages = new Subject<string>();

            var timer = Observable
                .Timer(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1), RxApp.MainThreadScheduler)
                .Scan(0, (acc, _) => ++acc)
                .Do(t => this.LogMessage("Timer tick " + t));

            this.timeActivated = this
                .GetIsActivated()
                .Select(isActivated => isActivated ? timer : Observable.Return(0))
                .Switch()
                .ToProperty(this, x => x.TimeActivated);
        }

        public ViewModelActivator Activator => this.activator;

        public IObservable<string> Messages => this.messages;

        public int TimeActivated => this.timeActivated.Value;

        public void LogMessage(string message) =>
            this.messages.OnNext(message);
    }
}