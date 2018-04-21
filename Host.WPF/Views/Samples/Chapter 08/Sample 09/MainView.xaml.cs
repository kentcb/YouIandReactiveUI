namespace Book.Views.Samples.Chapter08.Sample09
{
    using System.Reactive.Disposables;
    using ReactiveUI;
    using ViewModels.Samples.Chapter08.Sample09;

    public partial class MainView : ReactiveUserControl<MainViewModel>
    {
        public MainView()
        {
            InitializeComponent();

            this.loadAHButton.CommandParameter = new CharacterRange('A', 'H');
            this.loadIPButton.CommandParameter = new CharacterRange('I', 'P');
            this.loadQZButton.CommandParameter = new CharacterRange('Q', 'Z');

            this
                .WhenActivated(
                    disposables =>
                    {
                        this
                            .OneWayBind(this.ViewModel, x => x.Dinosaurs, x => x.dinosaursListBox.ItemsSource)
                            .DisposeWith(disposables);
                        this
                            .BindCommand(this.ViewModel, x => x.LoadDinosaursCommand, x => x.loadAHButton)
                            .DisposeWith(disposables);
                        this
                            .BindCommand(this.ViewModel, x => x.LoadDinosaursCommand, x => x.loadIPButton)
                            .DisposeWith(disposables);
                        this
                            .BindCommand(this.ViewModel, x => x.LoadDinosaursCommand, x => x.loadQZButton)
                            .DisposeWith(disposables);
                    });
        }
    }
}