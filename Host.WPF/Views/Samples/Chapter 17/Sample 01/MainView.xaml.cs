namespace Book.Views.Samples.Chapter17.Sample01
{
    using System.Reactive.Disposables;
    using ReactiveUI;
    using ViewModels.Samples.Chapter17.Sample01;

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
                            .OneWayBind(this.ViewModel, x => x.Router, x => x.routedViewHost.Router)
                            .DisposeWith(disposables);
                    });
        }
    }
}