namespace Book.Views.Samples.Chapter18.Sample06
{
    using System.Reactive.Disposables;
    using ReactiveUI;
    using ViewModels.Samples.Chapter18.Sample06;

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
                            .OneWayBind(this.ViewModel, x => x.Child, x => x.childViewModelViewHost.ViewModel)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.Messages, x => x.messagesTextBox.Text)
                            .DisposeWith(disposables);

                        this
                            .BindCommand(this.ViewModel, x => x.LoadChildCommand, x => x.loadChildButton)
                            .DisposeWith(disposables);
                        this
                            .BindCommand(this.ViewModel, x => x.UnloadChildCommand, x => x.unloadChildButton)
                            .DisposeWith(disposables);
                    });
        }
    }
}