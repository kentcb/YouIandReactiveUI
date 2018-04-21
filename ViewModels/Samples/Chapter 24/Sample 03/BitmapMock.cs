namespace Book.ViewModels.Samples.Chapter24.Sample03
{
    using System.IO;
    using System.Threading.Tasks;
    using PCLMock;
    using Splat;

    // I'm using PCLMock here (without code generation and hard-coded to loose behavior), but you can of course use whatever you fancy.
    public sealed class BitmapMock : MockBase<IBitmap>, IBitmap
    {
        public BitmapMock()
            : base(MockBehavior.Loose)
        {
            this.ConfigureLooseBehavior();
        }

        public float Width => this.Apply(x => x.Width);

        public float Height => this.Apply(x => x.Height);

        public void Dispose() =>
            this.Apply(x => x.Dispose());

        public Task Save(CompressedBitmapFormat format, float quality, Stream target) =>
            this.Apply(x => x.Save(format, quality, target));

        private void ConfigureLooseBehavior()
        {
            this
                .When(x => x.Save(It.IsAny<CompressedBitmapFormat>(), It.IsAny<float>(), It.IsAny<Stream>()))
                .Return(Task.CompletedTask);
        }
    }
}