namespace Book.ViewModels.Samples.Chapter24.Sample03
{
    using Genesis.TestUtil;

    public sealed class BitmapLoaderMockBuilder : IBuilder
    {
        private readonly BitmapLoaderMock bitmapLoader;

        public BitmapLoaderMockBuilder()
        {
            this.bitmapLoader = new BitmapLoaderMock();
        }

        public BitmapLoaderMock Build() =>
            this.bitmapLoader;

        public static implicit operator BitmapLoaderMock(BitmapLoaderMockBuilder builder) =>
            builder.Build();
    }
}