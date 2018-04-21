namespace Book.Views.Samples.Chapter17.Sample01
{
    using System.Reactive.Disposables;
    using ReactiveUI;
    using ViewModels.Samples.Chapter17.Sample01;

    public partial class BlurbView : ReactiveUserControl<BlurbViewModel>
    {
        public BlurbView()
        {
            InitializeComponent();

            this
                .WhenActivated(
                    disposables =>
                    {
                        this
                            .BindCommand(this.ViewModel, x => x.FavoriteDinosaurCommand, x => x.favoriteDinosaurHyperlink)
                            .DisposeWith(disposables);
                        this
                            .BindCommand(this.ViewModel, x => x.FavoriteScientistCommand, x => x.favoriteScientistHyperlink)
                            .DisposeWith(disposables);
                    });
        }
    }
}