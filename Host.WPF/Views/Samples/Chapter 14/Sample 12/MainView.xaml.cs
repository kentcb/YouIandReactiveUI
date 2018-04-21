namespace Book.Views.Samples.Chapter14.Sample12
{
    using ReactiveUI;
    using System.Reactive.Disposables;
    using ViewModels.Samples.Chapter14.Sample12;

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
                            .BindCommand(this.ViewModel, x => x.IncrementTicketCountCommand, x => x.incrementButton)
                            .DisposeWith(disposables);
                        this
                            .BindCommand(this.ViewModel, x => x.DecrementTicketCountCommand, x => x.decrementButton)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.TicketCount, x => x.ticketCountLabel.Content)
                            .DisposeWith(disposables);
                    });
        }
    }
}