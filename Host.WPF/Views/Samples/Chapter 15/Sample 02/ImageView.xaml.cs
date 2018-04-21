namespace Book.Views.Samples.Chapter15.Sample02
{
    using System.Reactive.Disposables;
    using ReactiveUI;
    using Splat;
    using ViewModels.Samples.Chapter15.Sample02;

    public partial class ImageView : ReactiveUserControl<DinosaurViewModel>
    {
        public ImageView()
        {
            InitializeComponent();

            this
                .WhenActivated(
                    disposables =>
                    {
                        this
                            .OneWayBind(this.ViewModel, x => x.Image, x => x.image.Source, x => x?.ToNative())
                            .DisposeWith(disposables);
                    });
        }
    }
}