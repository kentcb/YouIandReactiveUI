namespace Book.Views.Samples.Chapter04.Sample01
{
    using System.Reactive.Disposables;
    using global::Splat;
    using ReactiveUI;
    using ViewModels.Samples.Chapter04.Sample01;

    public partial class DinosaurView : ReactiveUserControl<DinosaurViewModel>
    {
        public DinosaurView()
        {
            InitializeComponent();

            this
                .WhenActivated(
                    disposables =>
                    {
                        // bind the IBitmap exposed by the view model to our Image. Note the use of ToNative to turn the platform-independent
                        // abstraction, IBitmap, into the platform-specific implementation
                        this
                            .OneWayBind(this.ViewModel, x => x.Image, x => x.dinosaurImage.Source, x => x?.ToNative())
                            .DisposeWith(disposables);
                    });
        }
    }
}