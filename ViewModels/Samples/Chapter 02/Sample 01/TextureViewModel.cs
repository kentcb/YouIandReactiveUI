namespace Book.ViewModels.Samples.Chapter02.Sample01
{
    public sealed class TextureViewModel
    {
        private readonly string resourceName;

        public TextureViewModel(string resourceName)
        {
            this.resourceName = resourceName;
        }

        public string ResourceName => this.resourceName;
    }
}