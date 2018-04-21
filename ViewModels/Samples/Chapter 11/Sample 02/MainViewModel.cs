namespace Book.ViewModels.Samples.Chapter11.Sample02
{
    using System.Reactive;
    using ReactiveUI;

    [Sample(
        "Dino chat: Service",
        @"This sample shows a chat system that forgoes `MessageBus` in preference to a purpose-built service named `ChatService`. Any number of room+user combinations can be added. When a user posts a message, that message will appear to all other users in the same room.")]
    public sealed class MainViewModel : ReactiveObject
    {
        private readonly ReactiveList<ChatViewModel> chats;
        private readonly ReactiveCommand<Unit, Unit> addChatCommand;
        private string room;
        private string user;

        public MainViewModel()
        {
            this.chats = new ReactiveList<ChatViewModel>();

            var canAddChat = this
                .WhenAnyValue(
                    x => x.Room,
                    x => x.User,
                    (room, user) => !string.IsNullOrWhiteSpace(room) && !string.IsNullOrWhiteSpace(user));
            this.addChatCommand = ReactiveCommand.Create(
                () => this.chats.Add(new ChatViewModel(this.Room, this.User)),
                canAddChat,
                RxApp.MainThreadScheduler);
        }

        public ReactiveCommand<Unit, Unit> AddChatCommand => this.addChatCommand;

        public IReactiveList<ChatViewModel> Chats => this.chats;

        public string Room
        {
            get => this.room;
            set => this.RaiseAndSetIfChanged(ref this.room, value);
        }

        public string User
        {
            get => this.user;
            set => this.RaiseAndSetIfChanged(ref this.user, value);
        }
    }
}