namespace Book.ViewModels.Samples.Chapter11.Sample01
{
    using System;
    using System.Linq;
    using System.Reactive;
    using System.Reactive.Linq;
    using ReactiveUI;

    public sealed class ChatViewModel : ReactiveObject
    {
        private readonly IMessageBus messageBus;
        private readonly string room;
        private readonly string user;
        private readonly ReactiveCommand<Unit, Unit> sendMessageCommand;
        private readonly IReactiveList<ChatMessageViewModel> chatMessages;
        private string message;

        public ChatViewModel(string room, string user)
            : this(MessageBus.Current, room, user)
        {
        }

        public ChatViewModel(IMessageBus messageBus, string room, string user)
        {
            this.messageBus = messageBus;
            this.room = room;
            this.user = user;
            this.chatMessages = new ReactiveList<ChatMessageViewModel>();

            var canSendMessage = this
                .WhenAnyValue(x => x.Message)
                .Select(x => !string.IsNullOrEmpty(x));
            this.sendMessageCommand = ReactiveCommand.Create(
                () => { },
                canSendMessage,
                RxApp.MainThreadScheduler);

            messageBus
                .Listen<ChatMessage>(this.room)
                .Select(x => new ChatMessageViewModel(x.User, x.Message))
                .Subscribe(this.chatMessages.Add);

            var messages = this
                .sendMessageCommand
                .Select(_ => new ChatMessage(this.user, this.Message))
                .Do(_ => this.Message = null);
            messageBus.RegisterMessageSource(messages, this.room);
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