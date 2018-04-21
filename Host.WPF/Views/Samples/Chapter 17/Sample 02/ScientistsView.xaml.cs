namespace Book.Views.Samples.Chapter17.Sample02
{
    using System.Reactive.Disposables;
    using ReactiveUI;
    using ViewModels.Samples.Chapter17.Sample02;

    public partial class ScientistsView : ReactiveUserControl<ScientistsViewModel>
    {
        public ScientistsView()
        {
            InitializeComponent();

            this
                .WhenActivated(
                    disposables =>
                    {
                        this
                            .OneWayBind(this.ViewModel, x => x.Scientists, x => x.scientistsListBox.ItemsSource)
                            .DisposeWith(disposables);
                        this
                            .Bind(this.ViewModel, x => x.SelectedScientist, x => x.scientistsListBox.SelectedItem)
                            .DisposeWith(disposables);
                    });
        }
    }
}