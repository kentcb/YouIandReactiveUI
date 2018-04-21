namespace Book.Views.Samples.Chapter15.Sample01
{
    using System.Reactive.Disposables;
    using ReactiveUI;
    using Splat;
    using ViewModels.Samples.Chapter15.Sample01;

    public partial class MainView : ReactiveUserControl<MainViewModel>
    {
        public MainView()
        {
            InitializeComponent();

            var viewLocator = Locator.Current.GetService<IViewLocator>();

            this
                .WhenActivated(
                    disposables =>
                    {
                        this
                            .OneWayBind(
                                this.ViewModel,
                                x => x.CountdownViewModel,
                                x => x.host.Content,
                                x =>
                                {
                                    var view = viewLocator.ResolveView(x);
                                    view.ViewModel = x;
                                    return view;
                                })
                            .DisposeWith(disposables);
                    });
        }
    }
}