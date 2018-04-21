namespace Book.Views.Samples.Chapter16.Sample03
{
    using System.Reactive.Disposables;
    using ReactiveUI;
    using Splat;
    using ViewModels.Samples.Chapter16.Sample03;

    public partial class DinosaurListItemView : ReactiveUserControl<DinosaurViewModel>
    {
        public DinosaurListItemView()
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
                            .OneWayBind(this.ViewModel, x => x.Image, x => x.image.Source, x => x?.ToNative())
                            .DisposeWith(disposables);
                    });
        }
    }
}