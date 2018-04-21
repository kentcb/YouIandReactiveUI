namespace Book.Views.Samples.Chapter14.Sample08
{
    using System.Reactive.Disposables;
    using ReactiveUI;
    using Splat;
    using ViewModels.Samples.Chapter14.Sample08;

    public partial class MainView : ReactiveUserControl<MainViewModel>
    {
        static MainView()
        {
            Locator.CurrentMutable.RegisterConstant(
                TimestampToStringConverter.Instance,
                typeof(IBindingTypeConverter));
            Locator.CurrentMutable.RegisterConstant(
                StringToTimestampConverter.Instance,
                typeof(IBindingTypeConverter));
        }

        public MainView()
        {
            InitializeComponent();

            this
                .WhenActivated(
                    disposables =>
                    {
                        this
                            .Bind(this.ViewModel, x => x.Time, x => x.timeTextBox.Text)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.Display, x => x.displayLabel.Content)
                            .DisposeWith(disposables);
                    });
        }
    }
}