namespace Book.ViewModels.Samples.Chapter17.Sample02
{
    using Book.ViewModels.Data;
    using ReactiveUI;
    using Splat;

    public sealed class DinosaurViewModel : ReactiveObject, IRoutableViewModel
    {
        private readonly Dinosaur model;
        private readonly IScreen hostScreen;
        private readonly ObservableAsPropertyHelper<IBitmap> image;

        public DinosaurViewModel(
            Dinosaur model,
            IScreen hostScreen)
        {
            this.model = model;
            this.hostScreen = hostScreen;
            this.image = model
                .GetBitmap()
                .ToProperty(this, x => x.Image);
        }

        public string UrlPathSegment => this.Name;

        public IScreen HostScreen => this.hostScreen;

        public string Name => this.model.Name;

        public Diet? Diet => this.model.Diet;

        public IBitmap Image => this.image.Value;
    }
}