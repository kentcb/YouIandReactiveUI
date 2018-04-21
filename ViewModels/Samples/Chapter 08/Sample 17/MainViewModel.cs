namespace Book.ViewModels.Samples.Chapter08.Sample17
{
    using System;
    using System.Linq;
    using System.Reactive;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;
    using ReactiveUI;

    [Sample(
        "Mutual exclusion",
        @"This sample demonstrates how multiple reactive commands can each become unavailable when any of the other commands is executing. That is, they are mutually exclusive.

The key is lifting the shared state into the `anyExecuting` subject, which tracks whether _any_ of the three commands are executing. The `noneExecuting` pipeline is a logical NOT of the `anyExecuting` value, so it is passed in as the `canExecute` pipeline for each command.")]
    public sealed class MainViewModel : ReactiveObject
    {
        private readonly ReactiveCommand<Unit, Unit> command1;
        private readonly ReactiveCommand<Unit, Unit> command2;
        private readonly ReactiveCommand<Unit, Unit> command3;

        public MainViewModel()
        {
            var anyExecuting = new BehaviorSubject<bool>(false);
            var noneExecuting = anyExecuting
                .Select(ae => !ae);
            this.command1 = ReactiveCommand.CreateFromObservable(
                () => this.Execute(),
                noneExecuting);
            this.command2 = ReactiveCommand.CreateFromObservable(
                () => this.Execute(),
                noneExecuting);
            this.command3 = ReactiveCommand.CreateFromObservable(
                () => this.Execute(),
                noneExecuting);

            Observable
                .Merge(
                    this.command1.IsExecuting,
                    this.command2.IsExecuting,
                    this.command3.IsExecuting)
                .Subscribe(anyExecuting);
        }

        public ReactiveCommand<Unit, Unit> Command1 => this.command1;

        public ReactiveCommand<Unit, Unit> Command2 => this.command2;

        public ReactiveCommand<Unit, Unit> Command3 => this.command3;

        private IObservable<Unit> Execute() =>
            Observable
                .Return(Unit.Default)
                .Delay(TimeSpan.FromSeconds(1));
    }
}