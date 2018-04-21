namespace Book.Views.Samples.Chapter14.Sample05
{
    using System.Reactive.Disposables;
    using ReactiveUI;
    using ViewModels.Samples.Chapter14.Sample05;

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
                            .OneWayBind(
                                this.ViewModel,
                                x => x.Time,
                                x => x.timeTextBlock.Text,
                                conversionHint: "TODAY, HH:mm:ss",
                                vmToViewConverterOverride: TimestampToStringConverter.Instance)
                            .DisposeWith(disposables);
                    });
        }
    }
}