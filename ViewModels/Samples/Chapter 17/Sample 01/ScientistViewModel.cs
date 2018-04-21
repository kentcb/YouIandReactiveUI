namespace Book.ViewModels.Samples.Chapter17.Sample01
{
    using System.Reactive;
    using Book.ViewModels.Data;
    using ReactiveUI;

    public sealed class ScientistViewModel : ReactiveObject, IRoutableViewModel
    {
        private readonly Scientist model;
        private readonly IScreen hostScreen;

        public ScientistViewModel(
            Scientist model,
            IScreen hostScreen)
        {
            this.model = model;
            this.hostScreen = hostScreen;
        }

        public string UrlPathSegment => this.Name;

        public IScreen HostScreen => this.hostScreen;

        public ReactiveCommand<Unit, Unit> BackCommand => this.hostScreen.Router.NavigateBack;

        public string Name => this.model.Name;

        public string Bio => this.model.Bio;

        public string ImageUri => this.model.ImageUri;
    }
}