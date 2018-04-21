namespace Book.Views.Samples.Chapter07.Sample02
{
    using System.Reactive.Disposables;
    using ReactiveUI;
    using ViewModels.Samples.Chapter07.Sample02;

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
                            .OneWayBind(this.ViewModel, x => x.Seconds, x => x.secondsRun.Text)
                            .DisposeWith(disposables);
                    });
        }
    }
}