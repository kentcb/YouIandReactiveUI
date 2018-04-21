namespace Book.Views.Samples.Chapter06.Sample04
{
    using System.Reactive.Disposables;
    using ReactiveUI;
    using ViewModels.Samples.Chapter06.Sample04;

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
                            .BindCommand(this.ViewModel, x => x.ResetDinosaurACommand, x => x.resetDinosaurAHyperlink)
                            .DisposeWith(disposables);
                        this
                            .Bind(this.ViewModel, x => x.DinosaurA.Weight, x => x.weightATextBox.Text)
                            .DisposeWith(disposables);
                        this
                            .Bind(this.ViewModel, x => x.DinosaurA.HasScales, x => x.hasScalesACheckBox.IsChecked)
                            .DisposeWith(disposables);
                        this
                            .Bind(this.ViewModel, x => x.DinosaurA.HasHorns, x => x.hasHornsACheckBox.IsChecked)
                            .DisposeWith(disposables);
                        this
                            .Bind(this.ViewModel, x => x.DinosaurA.HasSpikes, x => x.hasSpikesACheckBox.IsChecked)
                            .DisposeWith(disposables);
                        this
                            .Bind(this.ViewModel, x => x.DinosaurA.HasClub, x => x.hasClubACheckBox.IsChecked)
                            .DisposeWith(disposables);

                        this
                            .BindCommand(this.ViewModel, x => x.ResetDinosaurBCommand, x => x.resetDinosaurBHyperlink)
                            .DisposeWith(disposables);
                        this
                            .Bind(this.ViewModel, x => x.DinosaurB.Weight, x => x.weightBTextBox.Text)
                            .DisposeWith(disposables);
                        this
                            .Bind(this.ViewModel, x => x.DinosaurB.HasScales, x => x.hasScalesBCheckBox.IsChecked)
                            .DisposeWith(disposables);
                        this
                            .Bind(this.ViewModel, x => x.DinosaurB.HasHorns, x => x.hasHornsBCheckBox.IsChecked)
                            .DisposeWith(disposables);
                        this
                            .Bind(this.ViewModel, x => x.DinosaurB.HasSpikes, x => x.hasSpikesBCheckBox.IsChecked)
                            .DisposeWith(disposables);
                        this
                            .Bind(this.ViewModel, x => x.DinosaurB.HasClub, x => x.hasClubBCheckBox.IsChecked)
                            .DisposeWith(disposables);

                        this
                            .OneWayBind(this.ViewModel, x => x.Winner, x => x.winnerLabel.Content)
                            .DisposeWith(disposables);
                    });
        }
    }
}