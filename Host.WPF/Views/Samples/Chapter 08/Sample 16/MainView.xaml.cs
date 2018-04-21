namespace Book.Views.Samples.Chapter08.Sample16
{
    using System.Reactive.Disposables;
    using System.Windows;
    using ReactiveUI;
    using ViewModels.Samples.Chapter08.Sample16;

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
                            .OneWayBind(this.ViewModel, x => x.Roars, x => x.roarsListView.ItemsSource)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.IsInError, x => x.roarsListView.Visibility, x => x ? Visibility.Collapsed : Visibility.Visible)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.IsInError, x => x.errorTextBlock.Visibility, x => x ? Visibility.Visible : Visibility.Collapsed)
                            .DisposeWith(disposables);
                    });
        }
    }
}