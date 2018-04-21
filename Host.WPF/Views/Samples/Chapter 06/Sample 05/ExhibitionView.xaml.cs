namespace Book.Views.Samples.Chapter06.Sample05
{
    using System.Reactive.Disposables;
    using ReactiveUI;
    using ViewModels.Samples.Chapter06.Sample05;

    public partial class ExhibitionView : ReactiveUserControl<ExhibitionViewModel>
    {
        public ExhibitionView()
        {
            InitializeComponent();

            this
                .WhenActivated(
                    disposables =>
                    {
                        this
                            .Bind(this.ViewModel, x => x.Name, x => x.nameLabel.Content)
                            .DisposeWith(disposables);
                    });
        }
    }
}