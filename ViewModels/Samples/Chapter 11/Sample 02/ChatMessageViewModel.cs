namespace Book.ViewModels.Samples.Chapter11.Sample02
{
    using ReactiveUI;

    public sealed class ChatMessageViewModel : ReactiveObject
    {
        private readonly string user;
        private readonly string message;

        public ChatMessageViewModel(string user, string message)
        {
            this.user = user;
            this.message = message;
        }

        public string User => this.user;

        public string Message => this.message;
    }
}