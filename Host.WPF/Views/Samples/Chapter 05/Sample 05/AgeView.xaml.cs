namespace Book.Views.Samples.Chapter05.Sample05
{
    using System.Reactive.Disposables;
    using System.Windows.Media;
    using global::Splat;
    using ReactiveUI;
    using ViewModels.Samples.Chapter05.Sample05;

    public partial class AgeView : ReactiveUserControl<AgeViewModel>
    {
        public AgeView()
        {
            InitializeComponent();

            this
                .WhenActivated(
                    disposables =>
                    {
                        this
                            .OneWayBind(this.ViewModel, x => x.Display, x => x.displayTextBlock.Text)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.Color, x => x.swatchBorder.Background, x => new SolidColorBrush(x.ToNative()))
                            .DisposeWith(disposables);
                    });
        }
    }
}