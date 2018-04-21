namespace Book.Views.Samples.Chapter14.Sample04
{
    using System.Reactive.Disposables;
    using ReactiveUI;
    using ViewModels.Samples.Chapter14.Sample04;

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
                            .OneWayBind(this.ViewModel, x => x.Time, x => x.timeTextBlock.Text, vmToViewConverterOverride: TimestampToStringConverter.Instance)
                            .DisposeWith(disposables);
                    });
        }
    }
}