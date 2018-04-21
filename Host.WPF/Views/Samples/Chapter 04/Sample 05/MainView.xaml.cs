namespace Book.Views.Samples.Chapter04.Sample05
{
    using System.Reactive.Disposables;
    using ReactiveUI;
    using ViewModels.Samples.Chapter04.Sample05;

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
                            .Bind(this.ViewModel, x => x.Species, x => x.speciesTextBox.Text)
                            .DisposeWith(disposables);
                        this
                            .Bind(this.ViewModel, x => x.Location, x => x.locationTextBox.Text)
                            .DisposeWith(disposables);
                        this
                            .BindCommand(this.ViewModel, x => x.LogCommand, x => x.logButton)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.LogOutput, x => x.logOutputTextBox.Text)
                            .DisposeWith(disposables);
                    });
        }
    }
}