namespace Book.Views.Samples.Chapter11.Sample01
{
    using System.Reactive.Disposables;
    using ReactiveUI;
    using ViewModels.Samples.Chapter11.Sample01;

    public partial class ChatMessageView : ReactiveUserControl<ChatMessageViewModel>
    {
        public ChatMessageView()
        {
            InitializeComponent();

            this
                .WhenActivated(
                    disposables =>
                    {
                        this
                            .OneWayBind(this.ViewModel, x => x.User, x => x.userTextBlock.Text)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.Message, x => x.messageLabel.Content)
                            .DisposeWith(disposables);
                    });
        }
    }
}