namespace Book.ViewModels.Samples.Chapter17.Sample02
{
    using Book.ViewModels.Data;
    using ReactiveUI;

    public sealed class MuseumViewModel : ReactiveObject, IRoutableViewModel
    {
        private readonly Museum model;
        private readonly IScreen hostScreen;

        public MuseumViewModel(
            Museum model,
            IScreen hostScreen)
        {
            this.model = model;
            this.hostScreen = hostScreen;
        }

        public string UrlPathSegment => this.Name;

        public IScreen HostScreen => this.hostScreen;

        public string Name => this.model.Name;

        public string Location => this.model.Location;
    }
}