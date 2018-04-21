namespace Book.Views.Samples.Chapter08.Sample14
{
    using System;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using ReactiveUI;
    using ViewModels.Samples.Chapter08.Sample14;

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
                            .OneWayBind(this.ViewModel, x => x.RetryDisplay, x => x.retryDisplayLabel.Content)
                            .DisposeWith(disposables);
                        this
                            .ViewModel
                            .BookReservationCommand
                            .SelectMany(_ => this.ShowMessage("Booking successful!", ""))
                            .Subscribe()
                            .DisposeWith(disposables);
                    });
        }
    }
}