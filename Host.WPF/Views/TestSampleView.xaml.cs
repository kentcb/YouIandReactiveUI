namespace Book.Views
{
    using System.Reactive.Disposables;
    using ReactiveUI;
    using ViewModels;

    public partial class TestSampleView : ReactiveUserControl<TestSampleViewModel>
    {
        public TestSampleView()
        {
            InitializeComponent();

            this
                .WhenActivated(
                    disposables =>
                    {
                        this
                            .OneWayBind(this.ViewModel, x => x.Tests, x => x.testsListView.ItemsSource)
                            .DisposeWith(disposables);
                        this
                            .Bind(this.ViewModel, x => x.SelectedTest, x => x.testsListView.SelectedItem)
                            .DisposeWith(disposables);
                        this
                            .BindCommand(this.ViewModel, x => x.ExecuteAllTestsCommand, x => x.executeAllButton)
                            .DisposeWith(disposables);
                        this
                            .BindCommand(this.ViewModel, x => x.ExecuteSelectedTestCommand, x => x.executeButton)
                            .DisposeWith(disposables);
                    });
        }
    }
}