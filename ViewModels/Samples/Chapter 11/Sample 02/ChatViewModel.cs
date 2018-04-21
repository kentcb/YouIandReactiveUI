namespace Book.ViewModels.Samples.Chapter11.Sample02
{
    using System;
    using System.Linq;
    using System.Reactive;
    using System.Reactive.Linq;
    using ReactiveUI;

    public sealed class ChatViewModel : ReactiveObject
    {
        private readonly IChatService chatService;
        private readonly string room;
        private readonly string user;
        private readonly ReactiveCommand<Unit, Unit> sendMessageCommand;
        private readonly IReactiveList<ChatMessageViewModel> chatMessages;
        private string message;

        public ChatViewModel(string room, string user)
            : this(ChatService.Instance, room, user)
        {
        }

        public ChatViewModel(IChatService chatService, string room, string user)
        {
            this.chatService = chatService;
            this.room = room;
            this.user = user;
            this.chatMessages = new ReactiveList<ChatMessageViewModel>();

            var canSendMessage = this
                .WhenAnyValue(x => x.Message)
                .Select(x => !string.IsNullOrEmpty(x));
            this.sendMessageCommand = ReactiveCommand.Create(() => { }, canSendMessage, RxApp.MainThreadScheduler);

            chatService
                .Listen(this.room)
                .Select(x => new ChatMessageViewModel(x.User, x.Message))
                .Subscribe(this.chatMessages.Add);

            var messages = this
                .sendMessageCommand
                .Select(_ => new ChatMessage(this.user, this.Message))
                .Do(_ => this.Message = null);
            this.chatService.Publish(this.room, messages);
        }

        public ReactiveCommand<Unit, Unit> SendMessageCommand => this.sendMessageCommand;

        public string User => this.user;

        public string Room => this.room;

        public string Title => this.user + " [" + this.room + "]";

        public IReactiveList<ChatMessageViewModel> ChatMessages => this.chatMessages;

        public string Message
        {
            get => this.message;
            set => this.RaiseAndSetIfChanged(ref this.message, value);
        }
    }
}