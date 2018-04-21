namespace Book.ViewModels.Samples.Chapter08.Sample14
{
    using System;
    using System.Reactive;
    using System.Reactive.Linq;
    using ReactiveUI;

    [Sample(
        "Retry with Feedback",
        @"This sample demonstrates how a reactive command can include retry logic that is made apparent to the user.

When the `BookReservationCommand` fails, an automatic retry pipeline kicks in. In addition to retrying the command, it surfaces information about the retry attempt to the user.")]
    public sealed class MainViewModel : ReactiveObject
    {
        private const int secondsBeforeRetry = 5;
        private static readonly Random random = new Random();

        private readonly ReactiveCommand<Unit, Unit> bookReservationCommand;
        private readonly ObservableAsPropertyHelper<string> retryDisplay;

        public MainViewModel()
        {
            this.bookReservationCommand = ReactiveCommand.CreateFromObservable(
                () =>
                    Observable
                        .Defer(
                            () =>
                                Observable
                                    .Return(random.Next(0, 4))
                                    .Delay(TimeSpan.FromSeconds(1))
                                    .Select(value => value != 0)
                                    .Do(
                                        fail =>
                                        {
                                            if (fail)
                                            {
                                                throw new Exception("Failed.");
                                            }
                                        }))
                        .Select(_ => Unit.Default));

            var failures = this
                .bookReservationCommand
                .ThrownExceptions;

            var retryDemarcations = failures
                .Select(
                    _ =>
                        Observable
                            .Timer(TimeSpan.FromSeconds(secondsBeforeRetry))
                            .Select(__ => DemarcationPosition.End)
                            .StartWith(DemarcationPosition.Start))
                .Publish()
                .RefCount();

            retryDemarcations
                .Switch()
                .Where(position => position == DemarcationPosition.End)
                .Select(_ => Unit.Default)
                .InvokeCommand(this.bookReservationCommand);

            this.retryDisplay = retryDemarcations
                .Select(
                    _ =>
                        Observable
                            .Timer(TimeSpan.Zero, TimeSpan.FromSeconds(1), RxApp.MainThreadScheduler)
                            .Scan(-1, (acc, __) => ++acc)
                            .Select(seconds => secondsBeforeRetry - seconds))
                .Switch()
                .Select(
                    secondsUntilRetry =>
                    {
                        if (secondsUntilRetry < 0)
                        {
                            return null;
                        }

                        return $"Sorry, booking failed. Retrying for you in {secondsUntilRetry} seconds...";
                    })
                .ToProperty(this, x => x.RetryDisplay);
        }

        public ReactiveCommand<Unit, Unit> BookReservationCommand => this.bookReservationCommand;

        public string RetryDisplay => this.retryDisplay.Value;

        private enum DemarcationPosition
        {
            Start,
            End
        }
    }
}