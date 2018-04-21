namespace Book.Views.Samples.Chapter08.Sample17
{
    using System.Reactive.Disposables;
    using ReactiveUI;
    using ViewModels.Samples.Chapter08.Sample17;

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
                            .BindCommand(this.ViewModel, x => x.Command1, x => x.button1)
                            .DisposeWith(disposables);
                        this
                            .BindCommand(this.ViewModel, x => x.Command2, x => x.button2)
                            .DisposeWith(disposables);
                        this
                            .BindCommand(this.ViewModel, x => x.Command3, x => x.button3)
                            .DisposeWith(disposables);
                    });
        }
    }
}