namespace Book.Views.Samples.Chapter14.Sample13
{
    using ReactiveUI;
    using System.Reactive.Disposables;
    using System.Windows.Controls;
    using ViewModels.Samples.Chapter14.Sample13;

    public partial class MainView : ReactiveUserControl<MainViewModel>
    {
        public MainView()
        {
            InitializeComponent();

            this
                .WhenActivated(
                    disposables =>
                    {
                        this
                            .BindCommand(this.ViewModel, x => x.IncrementTicketCountCommand, x => x.incrementButton, toEvent: nameof(Button.MouseDoubleClick))
                            .DisposeWith(disposables);
                        this
                            .BindCommand(this.ViewModel, x => x.DecrementTicketCountCommand, x => x.decrementButton, toEvent: nameof(Button.MouseDoubleClick))
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.TicketCount, x => x.ticketCountLabel.Content)
                            .DisposeWith(disposables);
                    });
        }
    }
}