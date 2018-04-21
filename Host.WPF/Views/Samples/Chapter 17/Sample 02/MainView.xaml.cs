namespace Book.Views.Samples.Chapter17.Sample02
{
    using System.Reactive.Disposables;
    using ReactiveUI;
    using ViewModels.Samples.Chapter17.Sample02;

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
                            .BindCommand(this.ViewModel, x => x.NavigateBackCommand, x => x.backButton)
                            .DisposeWith(disposables);
                        this
                            .BindCommand(this.ViewModel, x => x.StartAgainCommand, x => x.startAgainButton)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.Title, x => x.titleRun.Text)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.Breadcrumb, x => x.breadcrumbRun.Text)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.Router, x => x.routedViewHost.Router)
                            .DisposeWith(disposables);
                    });
        }
    }
}