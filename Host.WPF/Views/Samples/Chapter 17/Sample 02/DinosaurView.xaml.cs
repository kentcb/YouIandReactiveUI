namespace Book.Views.Samples.Chapter17.Sample02
{
    using System.Reactive.Disposables;
    using ReactiveUI;
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
                    });
        }
    }
}