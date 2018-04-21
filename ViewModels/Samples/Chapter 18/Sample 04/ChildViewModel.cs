namespace Book.ViewModels.Samples.Chapter18.Sample04
{
    using System;
    using System.Reactive.Disposables;
    using System.Reactive.Subjects;
    using ReactiveUI;

    public sealed class ChildViewModel : ReactiveObject, ISupportsActivation
    {
        private readonly ViewModelActivator activator;
        private readonly Subject<string> messages;

        public ChildViewModel()
        {
            this.activator = new ViewModelActivator();
            this.messages = new Subject<string>();

            this
                .WhenActivated(
                    disposables =>
                    {
                        this.LogMessage("VM Activated");
                        Disposable
                            .Create(() => this.LogMessage("VM Deactivated"))
                            .DisposeWith(disposables);
                    });
        }

        public ViewModelActivator Activator => this.activator;

        public IObservable<string> Messages => this.messages;

        public void LogMessage(string message) =>
            this.messages.OnNext(message);
    }
}