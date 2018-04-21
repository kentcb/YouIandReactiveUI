namespace Book.ViewModels.Samples.Chapter08.Sample16
{
    public sealed class RoarViewModel
    {
        private readonly string user;
        private readonly string image;
        private readonly string text;

        public RoarViewModel(
            string user,
            string image,
            string text)
        {
            this.user = user;
            this.image = image;
            this.text = text;
        }

        public string User => this.user;

        public string Image => this.image;

        public string Text => this.text;
    }
}