namespace Book.ViewModels.Samples.Chapter16.Sample02
{
    using Book.ViewModels.Data;
    using ReactiveUI;
    using Splat;

    public abstract class DinosaurViewModel : ReactiveObject
    {
        private readonly Dinosaur model;
        private readonly ObservableAsPropertyHelper<IBitmap> image;

        public DinosaurViewModel(Dinosaur model)
        {
            this.model = model;
            this.image = model
                .GetBitmap()
                .ToProperty(this, x => x.Image);
        }

        public string Name => this.model.Name;

        public Period? Period => this.model.Period;

        public Era? Era => this.model.Era;

        public Diet? Diet => this.model.Diet;

        public IBitmap Image => this.image.Value;
    }
}