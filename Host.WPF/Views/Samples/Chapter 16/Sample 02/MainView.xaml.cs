namespace Book.Views.Samples.Chapter16.Sample02
{
    using System.Reactive.Disposables;
    using ReactiveUI;
    using ViewModels.Samples.Chapter16.Sample02;

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
                            .OneWayBind(this.ViewModel, x => x.Dinosaurs, x => x.dinosaursListBox.ItemsSource)
                            .DisposeWith(disposables);
                        this
                            .Bind(this.ViewModel, x => x.SelectedDinosaur, x => x.dinosaursListBox.SelectedItem)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.SelectedDinosaur, x => x.dinosaurHost.ViewModel)
                            .DisposeWith(disposables);
                    });
        }
    }
}