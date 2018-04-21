namespace Book.Views.Samples.Chapter15.Sample02
{
    using ReactiveUI;
    using Splat;
    using System.Reactive.Disposables;
    using ViewModels.Samples.Chapter15.Sample02;

    public partial class MainView : ReactiveUserControl<MainViewModel>
    {
        static MainView()
        {
            // Most views are correctly resolved by the convention-based view locator bundled with this sample solution.
            // However, this sample is demonstrating contracts which are not handled by the convention-based view locator.
            // Therefore, we register explicitly here (but you would usually do this during app bootstrapping)
            Locator.CurrentMutable.Register(
                () => new ImageView(),
                typeof(IViewFor<DinosaurViewModel>),
                "Image");
            Locator.CurrentMutable.Register(
                () => new DetailsView(),
                typeof(IViewFor<DinosaurViewModel>),
                "Details");
        }

        public MainView()
        {
            InitializeComponent();

            var viewLocator = Locator.Current.GetService<IViewLocator>();

            this
                .WhenActivated(
                    disposables =>
                    {
                        this
                            .BindCommand(this.ViewModel, x => x.ToggleViewModeCommand, x => x.toggleViewModeButton)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.ViewMode, x => x.viewModeLabel.Content, x => "Current view mode: " + x)
                            .DisposeWith(disposables);

                        this
                            .WhenAnyValue(
                                x => x.ViewModel.DinosaurViewModel,
                                x => x.ViewModel.ViewMode,
                                (viewModel, viewMode) =>
                                {
                                    if (viewModel == null)
                                    {
                                        return null;
                                    }

                                    var view = viewLocator.ResolveView(viewModel, viewMode.ToString());
                                    view.ViewModel = viewModel;
                                    return view;
                                })
                            .BindTo(this.host, x => x.Content)
                            .DisposeWith(disposables);
                    });
        }
    }
}