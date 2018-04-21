namespace Book.ViewModels.Samples.Chapter24.Sample03
{
    using System.IO;
    using System.Threading.Tasks;
    using PCLMock;
    using Splat;

    // I'm using PCLMock here (without code generation and hard-coded to loose behavior), but you can of course use whatever you fancy.
    public sealed class BitmapLoaderMock : MockBase<IBitmapLoader>, IBitmapLoader
    {
        public BitmapLoaderMock()
            : base(MockBehavior.Loose)
        {
            this.ConfigureLooseBehavior();
        }

        public IBitmap Create(float width, float height) =>
            this.Apply(x => x.Create(width, height));

        public Task<IBitmap> Load(Stream sourceStream, float? desiredWidth, float? desiredHeight) =>
            this.Apply(x => x.Load(sourceStream, desiredWidth, desiredHeight));

        public Task<IBitmap> LoadFromResource(string source, float? desiredWidth, float? desiredHeight) =>
            this.Apply(x => x.LoadFromResource(source, desiredWidth, desiredHeight));

        private void ConfigureLooseBehavior()
        {
            this
                .When(x => x.Create(It.IsAny<float>(), It.IsAny<float>()))
                .Return(() => new BitmapMock());
            this
                .When(x => x.Load(It.IsAny<Stream>(), It.IsAny<float?>(), It.IsAny<float?>()))
                .Return(() => Task.FromResult<IBitmap>(new BitmapMock()));
            this
                .When(x => x.LoadFromResource(It.IsAny<string>(), It.IsAny<float?>(), It.IsAny<float?>()))
                .Return(() => Task.FromResult<IBitmap>(new BitmapMock()));
        }
    }
}