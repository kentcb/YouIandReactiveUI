namespace Book.ViewModels.Samples.Chapter18.Sample01
{
    using System;
    using System.Reactive.Subjects;
    using ReactiveUI;

    public sealed class ChildViewModel : ReactiveObject
    {
        private readonly string name;
        private readonly Subject<string> messages;

        public ChildViewModel(string name)
        {
            this.name = name;
            this.messages = new Subject<string>();
        }

        public string Name => this.name;

        public IObservable<string> Messages => this.messages;

        public void LogMessage(string message) =>
            this.messages.OnNext(this.name + ": " + message);
    }
}