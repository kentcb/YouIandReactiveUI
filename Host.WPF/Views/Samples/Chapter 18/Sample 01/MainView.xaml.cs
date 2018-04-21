namespace Book.Views.Samples.Chapter18.Sample01
{
    using System;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Windows.Controls.Primitives;
    using ReactiveUI;
    using ViewModels.Samples.Chapter18.Sample01;

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
                            .OneWayBind(this.ViewModel, x => x.SelectedChild, x => x.viewModelViewHost.ViewModel)
                            .DisposeWith(disposables);

                        this
                            .OneWayBind(this.ViewModel, x => x.Messages, x => x.messagesTextBox.Text)
                            .DisposeWith(disposables);
                        this
                            .BindCommand(this.ViewModel, x => x.SelectChild1Command, x => x.selectChild1Button)
                            .DisposeWith(disposables);
                        this
                            .BindCommand(this.ViewModel, x => x.SelectChild2Command, x => x.selectChild2Button)
                            .DisposeWith(disposables);
                        this
                            .messagesTextBox
                            .Events()
                            .TextChanged
                            .Do(_ => this.messagesTextBox.ScrollToEnd())
                            .Subscribe()
                            .DisposeWith(disposables);
                    });
        }
    }
}