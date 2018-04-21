namespace Book.Views.Samples.Chapter08.Sample15
{
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using global::Splat;
    using ReactiveUI;
    using ViewModels.Samples.Chapter08.Sample15;

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
                            .BindCommand(this.ViewModel, x => x.CloneCommand, x => x.button)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.Image, x => x.dinosaurImage.Source, x => x?.ToNative())
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.Image, x => x.dinosaurCloneImage.Source, x => x?.ToNative())
                            .DisposeWith(disposables);
                        this
                            .WhenAnyObservable(x => x.ViewModel.CloneCommand)
                            .Select(progress => progress.PercentComplete / 100d)
                            .BindTo(this, x => x.dinosaurCloneImage.Opacity)
                            .DisposeWith(disposables);
                        this
                            .WhenAnyObservable(x => x.ViewModel.CloneCommand)
                            .Select(progress => progress.StageName)
                            .BindTo(this, x => x.stageNameLabel.Content)
                            .DisposeWith(disposables);
                        this
                            .WhenAnyObservable(x => x.ViewModel.CloneCommand)
                            .Select(progress => progress.PercentStageComplete)
                            .BindTo(this, x => x.stageProgress.Value)
                            .DisposeWith(disposables);
                        this
                            .WhenAnyObservable(x => x.ViewModel.CloneCommand)
                            .Select(progress => progress.PercentComplete)
                            .BindTo(this, x => x.progress.Value)
                            .DisposeWith(disposables);
                    });
        }
    }
}