namespace Book.ViewModels.Samples.Chapter17.Sample01
{
    using System.Reactive;
    using Book.ViewModels.Data;
    using ReactiveUI;

    public sealed class DinosaurViewModel : ReactiveObject, IRoutableViewModel
    {
        private readonly Dinosaur model;
        private readonly IScreen hostScreen;

        public DinosaurViewModel(
            Dinosaur model,
            IScreen hostScreen)
        {
            this.model = model;
            this.hostScreen = hostScreen;
        }

        public string UrlPathSegment => this.Name;

        public IScreen HostScreen => this.hostScreen;

        public ReactiveCommand<Unit, Unit> BackCommand => this.hostScreen.Router.NavigateBack;

        public string Name => this.model.Name;

        public Diet? Diet => this.model.Diet;
    }
}