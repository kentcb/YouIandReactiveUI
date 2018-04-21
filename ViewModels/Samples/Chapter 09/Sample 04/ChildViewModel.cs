namespace Book.ViewModels.Samples.Chapter09.Sample04
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reactive;
    using System.Reactive.Linq;
    using ReactiveUI;

    public sealed class ChildViewModel : ReactiveObject
    {
        private static readonly Random random = new Random();
        private readonly ReactiveCommand<Unit, Unit> bookReservationCommand;

        public ChildViewModel()
        {
            this.bookReservationCommand = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    var r = random.Next(0, 3);

                    if (r == 0)
                    {
                        return Observable
                            .Throw<Unit>(new Exception());
                    }
                    else if (r == 1)
                    {
                        return Observable
                            .Throw<Unit>(new IOException());
                    }

                    return Observable
                        .Return(Unit.Default);
                });

                this
                    .bookReservationCommand
                    .ThrownExceptions
                    .SelectMany(SharedInteractions.UnhandledException.Handle)
                    .Subscribe();
        }

        public ReactiveCommand<Unit, Unit> BookReservationCommand => this.bookReservationCommand;
    }
}