namespace Book.Views.Samples.Chapter14.Sample02
{
    using System.Reactive.Disposables;
    using ReactiveUI;
    using ViewModels.Samples.Chapter14.Sample02;

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
                            .OneWayBind(this.ViewModel, x => x.Time, x => x.timeLabel.Content, x => x.ToString("HH:mm:ss"))
                            .DisposeWith(disposables);
                    });
        }
    }
}