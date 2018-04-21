namespace Book.ViewModels.Samples.Chapter04.Sample01
{
    using Data;
    using global::Splat;
    using ReactiveUI;

    public sealed class DinosaurViewModel : ReactiveObject
    {
        private readonly ObservableAsPropertyHelper<IBitmap> image;

        public DinosaurViewModel(Dinosaur model)
        {
            this.image = model
                .GetBitmap()
                .ToProperty(this, x => x.Image);
        }

        public IBitmap Image => this.image.Value;
    }
}