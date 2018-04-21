namespace Book.Views.Samples.Chapter05.Sample05
{
    using System.Reactive.Disposables;
    using ReactiveUI;
    using ViewModels.Samples.Chapter05.Sample05;

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
                            .Bind(this.ViewModel, x => x.Dinosaur.Name, x => x.nameTextBox.Text)
                            .DisposeWith(disposables);
                        this
                            .Bind(this.ViewModel, x => x.Dinosaur.Weight, x => x.weightTextBox.Text)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.Ages, x => x.agesListBox.ItemsSource)
                            .DisposeWith(disposables);
                        this
                            .Bind(this.ViewModel, x => x.Dinosaur.Age, x => x.agesListBox.SelectedItem)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.Output, x => x.outputTextBox.Text)
                            .DisposeWith(disposables);
                    });
        }
    }
}