namespace Book.Views.Samples.Chapter14.Sample03
{
    using System.Reactive.Disposables;
    using ReactiveUI;
    using Splat;
    using ViewModels.Samples.Chapter14.Sample03;

    public partial class MainView : ReactiveUserControl<MainViewModel>
    {
        static MainView()
        {
            // You'd normally perform this registration during app startup, not in the view. It's here for illustrative
            // purposes and to keep the sample self-contained.
            Locator.CurrentMutable.RegisterConstant(
                TimestampToStringConverter.Instance,
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
                            .OneWayBind(this.ViewModel, x => x.Time, x => x.timeTextBlock.Text)
                            .DisposeWith(disposables);
                    });
        }
    }
}