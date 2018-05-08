namespace Book.Views.Samples.Chapter17.Sample02
{
    using ReactiveUI;
    using Splat;
    using System.Reactive.Disposables;
    using ViewModels.Samples.Chapter17.Sample02;

    public partial class DinosaurView : ReactiveUserControl<DinosaurViewModel>
    {
        public DinosaurView()
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
                            .OneWayBind(this.ViewModel, x => x.Diet, x => x.dietLabel.Content)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.Image, x => x.image.Source, x => x?.ToNative())
                            .DisposeWith(disposables);
                    });
        }
    }
}