namespace Book.Views.Samples.Chapter14.Sample10
{
    using System.Reactive.Disposables;
    using ReactiveUI;
    using ViewModels.Samples.Chapter14.Sample10;

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
                            .Bind(
                                this.ViewModel,
                                x => x.Time,
                                x => x.timeTextBox.Text,
                                vmToViewConverterOverride: TimestampToStringConverter.Instance,
                                viewToVMConverterOverride: StringToTimestampConverter.Instance,
                                conversionHint: "TODAY, HH:mm:ss")
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.Display, x => x.displayLabel.Content)
                            .DisposeWith(disposables);
                    });
        }
    }
}