namespace Book.Views.Samples.Chapter07.Sample03
{
    using System.Reactive.Disposables;
    using ReactiveUI;
    using ViewModels.Samples.Chapter07.Sample03;

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
                            .OneWayBind(this.ViewModel, x => x.Message, x => x.messageTextBlock.Text)
                            .DisposeWith(disposables);
                    });
        }
    }
}