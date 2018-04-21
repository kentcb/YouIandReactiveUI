namespace Book.Views.Samples.Chapter18.Sample04
{
    using System.Reactive.Disposables;
    using ReactiveUI;
    using ViewModels.Samples.Chapter18.Sample04;

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
                            .OneWayBind(this.ViewModel, x => x.LeftChild, x => x.leftChildViewModelViewHost.ViewModel)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.RightChild, x => x.rightChildViewModelViewHost.ViewModel)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.Messages, x => x.messagesTextBox.Text)
                            .DisposeWith(disposables);

                        this
                            .BindCommand(this.ViewModel, x => x.LoadLeftChildCommand, x => x.loadLeftChildButton)
                            .DisposeWith(disposables);
                        this
                            .BindCommand(this.ViewModel, x => x.UnloadLeftChildCommand, x => x.unloadLeftChildButton)
                            .DisposeWith(disposables);
                        this
                            .BindCommand(this.ViewModel, x => x.LoadRightChildCommand, x => x.loadRightChildButton)
                            .DisposeWith(disposables);
                        this
                            .BindCommand(this.ViewModel, x => x.UnloadRightChildCommand, x => x.unloadRightChildButton)
                            .DisposeWith(disposables);
                    });
        }
    }
}