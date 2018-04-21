namespace Book.Views.Samples.Chapter10.Sample01
{
    using System.Reactive.Disposables;
    using ReactiveUI;
    using ViewModels.Samples.Chapter10.Sample01;

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
                            .OneWayBind(this.ViewModel, x => x.Count, x => x.countLabel.Content, x => $"There are {x} dinosaurs in the list.")
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.Dinosaurs, x => x.dinosaursListBox.ItemsSource)
                            .DisposeWith(disposables);
                    });
        }
    }
}