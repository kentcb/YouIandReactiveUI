namespace Book.Views.Samples.Chapter04.Sample03
{
    using System.Reactive.Disposables;
    using System.Windows.Media;
    using global::Splat;
    using ReactiveUI;
    using ViewModels.Samples.Chapter04.Sample03;

    public partial class ColorKeyView : ReactiveUserControl<ColorKeyViewModel>
    {
        public ColorKeyView()
        {
            InitializeComponent();

            this
                .WhenActivated(
                    disposables =>
                    {
                        this
                            .OneWayBind(this.ViewModel, x => x.Era, x => x.eraTextBlock.Text)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.Period, x => x.periodTextBlock.Text)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.Color, x => x.colorBorder.Background, x => new SolidColorBrush(x.ToNative()))
                            .DisposeWith(disposables);
                    });
        }
    }
}