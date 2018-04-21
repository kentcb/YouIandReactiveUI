namespace Book.Views.Samples.Chapter15.Sample02
{
    using System.Reactive.Disposables;
    using ReactiveUI;
    using ViewModels.Samples.Chapter15.Sample02;

    public partial class DetailsView : ReactiveUserControl<DinosaurViewModel>
    {
        public DetailsView()
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
                            .OneWayBind(this.ViewModel, x => x.Period, x => x.periodLabel.Content)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.Era, x => x.eraLabel.Content)
                            .DisposeWith(disposables);
                    });
        }
    }
}