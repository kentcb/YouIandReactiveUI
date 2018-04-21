namespace Book.Views.Samples.Chapter18.Sample02
{
    using System.Reactive.Disposables;
    using ReactiveUI;
    using ViewModels.Samples.Chapter18.Sample02;

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
                            .OneWayBind(this.ViewModel, x => x.SelectedChild, x => x.viewModelViewHost.ViewModel)
                            .DisposeWith(disposables);
                        this
                            .BindCommand(this.ViewModel, x => x.SelectChild1Command, x => x.selectChild1Button)
                            .DisposeWith(disposables);
                        this
                            .BindCommand(this.ViewModel, x => x.SelectChild2Command, x => x.selectChild2Button)
                            .DisposeWith(disposables);
                    });
        }
    }
}