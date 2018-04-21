namespace Book.ViewModels.Samples.Chapter08.Sample13
{
    using System;
    using System.Reactive;
    using System.Reactive.Linq;
    using ReactiveUI;

    [Sample(
        "Retry (Transparent)",
        @"This sample demonstrates how a reactive command can include transparent retry logic.

The logic for the `BookReservationCommand` includes a `Retry` operator. Since the booking logic is inherently unreliable, this gives our users a better chance of making a successful booking. There is no indication to the user at all that the retry is taking place - it just happens automatically as required.")]
    public sealed class MainViewModel : ReactiveObject
    {
        private static readonly Random random = new Random();

        private readonly ReactiveCommand<Unit, Unit> bookReservationCommand;
        private readonly ObservableAsPropertyHelper<int> executionCount;
        private readonly ObservableAsPropertyHelper<int> failureCount;

        public MainViewModel()
        {
            this.bookReservationCommand = ReactiveCommand.CreateFromObservable(
                () =>
                    Observable
                        .Defer(
                            () =>
                                Observable
                                    .Return(random.Next(0, 2))
                                    .Delay(TimeSpan.FromMilliseconds(50))
                                    .Select(value => value != 0)
                                    .Do(
                                        fail =>
                                        {
                                            if (fail)
                                            {
                                                throw new Exception("Failed.");
                                            }
                                        }))
                        .Retry(3)
                        .Select(_ => Unit.Default));
            this.executionCount = this
                .bookReservationCommand
                .Scan(0, (acc, _) => ++acc)
                .ToProperty(this, x => x.ExecutionCount);
            this.failureCount = this
                .bookReservationCommand
                .ThrownExceptions
                .Scan(0, (acc, _) => ++acc)
                .ToProperty(this, x => x.FailureCount);
        }

        public ReactiveCommand<Unit, Unit> BookReservationCommand => this.bookReservationCommand;

        public int ExecutionCount => this.executionCount.Value;

        public int FailureCount => this.failureCount.Value;
    }
}