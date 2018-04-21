namespace Book.Views.Samples.Chapter17.Sample02
{
    using System.Reactive.Disposables;
    using System.Windows.Media.Imaging;
    using ReactiveUI;
    using ViewModels.Samples.Chapter17.Sample02;

    public partial class ScientistView : ReactiveUserControl<ScientistViewModel>
    {
        public ScientistView()
        {
            InitializeComponent();

            this
                .WhenActivated(
                    disposables =>
                    {
                        this
                            .OneWayBind(this.ViewModel, x => x.Name, x => x.nameLabel.Content)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.Bio, x => x.bioTextBlock.Text)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.ImageUri, x => x.image.Source, x => x == null ? null : BitmapFrame.Create(new System.Uri(x)))
                            .DisposeWith(disposables);
                    });
        }
    }
}