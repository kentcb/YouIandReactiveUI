namespace Book.ViewModels.Samples.Chapter18.Sample05
{
    using System;
    using System.Reactive.Subjects;
    using ReactiveUI;

    public sealed class ChildViewModel : ReactiveObject
    {
        private readonly Subject<string> messages;

        public ChildViewModel()
        {
            this.messages = new Subject<string>();
        }

        public IObservable<string> Messages => this.messages;

        public void LogMessage(string message) =>
            this.messages.OnNext(message);
    }
}