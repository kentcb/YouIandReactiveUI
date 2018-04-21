namespace Book.Views.Samples.Chapter16.Sample04
{
    using System.Reactive.Disposables;
    using ReactiveUI;
    using ViewModels.Samples.Chapter16.Sample04;

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
                    });
        }
    }
}