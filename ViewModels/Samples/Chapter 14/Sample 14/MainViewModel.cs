namespace Book.ViewModels.Samples.Chapter14.Sample14
{
    using ReactiveUI;
    using System.Reactive.Linq;
    using System.Windows.Input;

    [Sample(
        "BindCommand: parameters",
        @"This sample demonstrates the means by which parameters can be passed to `BindCommand`. The view model has only a single command, which takes a parameter indicating a delta by which the `TicketCount` property should be modified when the command is executed. The view binds several buttons to this command, each with a different parameter value.")]
    public sealed class MainViewModel : ReactiveObject
    {
        private readonly ReactiveCommand<int, int> changeTicketCountCommand;
        private readonly ObservableAsPropertyHelper<int> ticketCount;

        public MainViewModel()
        {
            this.changeTicketCountCommand = ReactiveCommand.Create<int, int>(x => x);
            this.ticketCount = this
                .changeTicketCountCommand
                .Scan(
                    0,
                    (acc, next) => acc += next)
                .ToProperty(this, x => x.TicketCount);
        }

        public ICommand ChangeTicketCountCommand => this.changeTicketCountCommand;

        public int TicketCount => this.ticketCount.Value;
    }
}