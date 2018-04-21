namespace Book.Views.Samples.Chapter11.Sample01
{
    using System.Reactive.Disposables;
    using ReactiveUI;
    using ViewModels.Samples.Chapter11.Sample01;

    public partial class MainView : ReactiveUserControl<MainViewModel>
    {
        public MainView()
        {
            InitializeComponent();

            this
                .WhenActivated(
                    disposables =>
                    {
                        this
                            .Bind(this.ViewModel, x => x.Room, x => x.roomTextBox.Text)
                            .DisposeWith(disposables);
                        this
                            .Bind(this.ViewModel, x => x.User, x => x.userTextBox.Text)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.Chats, x => x.chatsItemsControl.ItemsSource)
                            .DisposeWith(disposables);
                        this
                            .BindCommand(this.ViewModel, x => x.AddChatCommand, x => x.startChatButton)
                            .DisposeWith(disposables);
                    });
        }
    }
}