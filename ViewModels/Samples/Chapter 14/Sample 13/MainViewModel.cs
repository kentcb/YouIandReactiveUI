namespace Book.ViewModels.Samples.Chapter14.Sample13
{
    using ReactiveUI;
    using System.Reactive;
    using System.Reactive.Linq;

    [Sample(
        "BindCommand: custom event",
        @"This sample demonstrates the use of `BindCommand` with a custom event. In this case, the user is required to double-click the buttons in order to execute their respective commands.")]
    public sealed class MainViewModel : ReactiveObject
    {
        private readonly ReactiveCommand<Unit, Unit> incrementTicketCountCommand;
        private readonly ReactiveCommand<Unit, Unit> decrementTicketCountCommand;
        private readonly ObservableAsPropertyHelper<int> ticketCount;

        public MainViewModel()
        {
            this.incrementTicketCountCommand = ReactiveCommand.Create(() => { });
            this.decrementTicketCountCommand = ReactiveCommand.Create(() => { });
            this.ticketCount = Observable
                .Merge(
                    this.incrementTicketCountCommand.Select(_ => 1),
                    this.decrementTicketCountCommand.Select(_ => -1))
                .Scan(
                    0,
                    (acc, next) => acc += next)
                .ToProperty(this, x => x.TicketCount);
        }

        public ReactiveCommand<Unit, Unit> IncrementTicketCountCommand => this.incrementTicketCountCommand;

        public ReactiveCommand<Unit, Unit> DecrementTicketCountCommand => this.decrementTicketCountCommand;

        public int TicketCount => this.ticketCount.Value;
    }
}