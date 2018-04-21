namespace Book.ViewModels.Samples.Chapter17.Sample01
{
    using System.Linq;
    using System.Reactive;
    using System.Reactive.Linq;
    using ReactiveUI;

    public sealed class BlurbViewModel : ReactiveObject, IRoutableViewModel
    {
        private readonly IScreen hostScreen;
        private readonly ReactiveCommand<Unit, Unit> favoriteDinosaurCommand;
        private readonly ReactiveCommand<Unit, Unit> favoriteScientistCommand;

        public BlurbViewModel(
            IScreen hostScreen)
        {
            this.hostScreen = hostScreen;

            var favoriteDinosaur = new DinosaurViewModel(
                Data
                    .Dinosaurs
                    .All
                    .Where(dinosaur => dinosaur.Name == "Spinosaurus")
                    .Single(),
                this.hostScreen);
            var favoriteScientist = new ScientistViewModel(
                Data
                    .Scientists
                    .All
                    .Where(scientist => scientist.Name.Contains("Dawkins"))
                    .Single(),
                this.hostScreen);

            this.favoriteDinosaurCommand = ReactiveCommand.CreateFromObservable(
                () => this.hostScreen.Router.Navigate.Execute(favoriteDinosaur).Select(_ => Unit.Default));
            this.favoriteScientistCommand = ReactiveCommand.CreateFromObservable(
                () => this.hostScreen.Router.Navigate.Execute(favoriteScientist).Select(_ => Unit.Default));
        }

        public string UrlPathSegment => "Blurb";

        public IScreen HostScreen => this.hostScreen;

        public ReactiveCommand<Unit, Unit> FavoriteDinosaurCommand => this.favoriteDinosaurCommand;

        public ReactiveCommand<Unit, Unit> FavoriteScientistCommand => this.favoriteScientistCommand;
    }
}