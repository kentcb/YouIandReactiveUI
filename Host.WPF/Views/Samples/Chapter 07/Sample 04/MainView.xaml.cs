namespace Book.Views.Samples.Chapter07.Sample04
{
    using System.Reactive.Disposables;
    using ReactiveUI;
    using ViewModels.Samples.Chapter07.Sample04;

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
                            .OneWayBind(this.ViewModel, x => x.Dinosaurs, x => x.dinosaursListView.ItemsSource)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.SubscriptionCount, x => x.subscriptionCountRun.Text)
                            .DisposeWith(disposables);
                    });
        }
    }
}