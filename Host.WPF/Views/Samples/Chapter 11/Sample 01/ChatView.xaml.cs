namespace Book.Views.Samples.Chapter11.Sample01
{
    using System.Reactive.Disposables;
    using ReactiveUI;
    using ViewModels.Samples.Chapter11.Sample01;

    public partial class ChatView : ReactiveUserControl<ChatViewModel>
    {
        public ChatView()
        {
            InitializeComponent();

            this
                .WhenActivated(
                    disposables =>
                    {
                        this
                            .OneWayBind(this.ViewModel, x => x.Title, x => x.titleLabel.Content)
                            .DisposeWith(disposables);
                        this
                            .Bind(this.ViewModel, x => x.Message, x => x.messageTextBox.Text)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.ChatMessages, x => x.chatMessagesListBox.ItemsSource)
                            .DisposeWith(disposables);
                        this
                            .BindCommand(this.ViewModel, x => x.SendMessageCommand, x => x.sendMessageButton)
                            .DisposeWith(disposables);
                    });
        }
    }
}