namespace Book.Views.Samples.Chapter04.Sample01
{
    using System.Reactive.Disposables;
    using ReactiveUI;
    using ViewModels.Samples.Chapter04.Sample01;

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
                            .OneWayBind(this.ViewModel, x => x.Dinosaurs, x => x.flipView.ItemsSource)
                            .DisposeWith(disposables);
                    });
        }
    }
}