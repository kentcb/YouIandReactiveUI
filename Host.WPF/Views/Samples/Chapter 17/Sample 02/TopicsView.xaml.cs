namespace Book.Views.Samples.Chapter17.Sample02
{
    using System.Reactive.Disposables;
    using ReactiveUI;
    using ViewModels.Samples.Chapter17.Sample02;

    public partial class TopicsView : ReactiveUserControl<TopicsViewModel>
    {
        public TopicsView()
        {
            InitializeComponent();

            this
                .WhenActivated(
                    disposables =>
                    {
                        this
                            .OneWayBind(this.ViewModel, x => x.Topics, x => x.topicsListBox.ItemsSource)
                            .DisposeWith(disposables);
                        this
                            .Bind(this.ViewModel, x => x.SelectedTopic, x => x.topicsListBox.SelectedItem)
                            .DisposeWith(disposables);
                    });
        }
    }
}