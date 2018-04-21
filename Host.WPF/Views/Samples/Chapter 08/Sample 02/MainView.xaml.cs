namespace Book.Views.Samples.Chapter08.Sample02
{
    using System.Reactive.Disposables;
    using ReactiveUI;
    using ViewModels.Samples.Chapter08.Sample02;

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
                            .Bind(this.ViewModel, x => x.Name, x => x.nameTextBox.Text)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.Dinosaurs, x => x.dinosaursListBox.ItemsSource)
                            .DisposeWith(disposables);
                        this
                            .Bind(this.ViewModel, x => x.SelectedDinosaur, x => x.dinosaursListBox.SelectedItem)
                            .DisposeWith(disposables);
                        this
                            .BindCommand(this.ViewModel, x => x.AddDinosaurCommand, x => x.addDinosaurButton)
                            .DisposeWith(disposables);
                        this
                            .BindCommand(this.ViewModel, x => x.DeleteDinosaurCommand, x => x.deleteDinosaurButton)
                            .DisposeWith(disposables);
                    });
        }
    }
}