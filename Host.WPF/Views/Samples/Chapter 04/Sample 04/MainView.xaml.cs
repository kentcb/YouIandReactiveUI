namespace Book.Views.Samples.Chapter04.Sample04
{
    using System.Reactive.Disposables;
    using ReactiveUI;
    using ViewModels.Samples.Chapter04.Sample04;

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
                            .OneWayBind(this.ViewModel, x => x.RegisterExhibition, x => x.registerExhibitionViewModelViewHost.ViewModel)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.ListExhibitions, x => x.listExhibitionsViewModelViewHost.ViewModel)
                            .DisposeWith(disposables);
                    });
        }
    }
}