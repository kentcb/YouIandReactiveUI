namespace Book.Views.Samples.Chapter14.Sample14
{
    using ReactiveUI;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using ViewModels.Samples.Chapter14.Sample14;

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
                            .BindCommand(
                                this.ViewModel,
                                x => x.ChangeTicketCountCommand,
                                x => x.add1Button,
                                withParameter: Observable.Return(1))
                            .DisposeWith(disposables);
                        this
                            .BindCommand(
                                this.ViewModel,
                                x => x.ChangeTicketCountCommand,
                                x => x.add5Button,
                                withParameter: Observable.Return(5))
                            .DisposeWith(disposables);
                        this
                            .BindCommand(
                                this.ViewModel,
                                x => x.ChangeTicketCountCommand,
                                x => x.remove5Button,
                                withParameter: Observable.Return(-5))
                            .DisposeWith(disposables);
                        this
                            .BindCommand(
                                this.ViewModel,
                                x => x.ChangeTicketCountCommand,
                                x => x.remove1Button,
                                withParameter: Observable.Return(-1))
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.TicketCount, x => x.ticketCountLabel.Content)
                            .DisposeWith(disposables);
                    });
        }
    }
}