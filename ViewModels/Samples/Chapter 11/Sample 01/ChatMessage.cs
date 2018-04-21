namespace Book.ViewModels.Samples.Chapter11.Sample01
{
    public sealed class ChatMessage
    {
        private readonly string user;
        private readonly string message;

        public ChatMessage(string user, string message)
        {
            this.user = user;
            this.message = message;
        }

        public string User => this.user;

        public string Message => this.message;
    }
}