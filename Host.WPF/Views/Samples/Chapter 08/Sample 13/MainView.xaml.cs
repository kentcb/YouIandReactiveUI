namespace Book.Views.Samples.Chapter08.Sample13
{
    using System.Reactive.Disposables;
    using ReactiveUI;
    using ViewModels.Samples.Chapter08.Sample13;

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
                            .BindCommand(this.ViewModel, x => x.BookReservationCommand, x => x.button)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.ExecutionCount, x => x.executionCountLabel.Content, x => "Executed " + x + " times.")
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.FailureCount, x => x.failureCountLabel.Content, x => "Failed " + x + " times.")
                            .DisposeWith(disposables);
                    });
        }
    }
}