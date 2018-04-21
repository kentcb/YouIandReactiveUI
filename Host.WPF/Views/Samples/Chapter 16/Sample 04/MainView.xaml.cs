namespace Book.Views.Samples.Chapter16.Sample04
{
    using System.Reactive.Disposables;
    using ReactiveUI;
    using Splat;
    using ViewModels.Samples.Chapter16.Sample04;

    public partial class MainView : ReactiveUserControl<MainViewModel>
    {
        static MainView()
        {
            // Most views are correctly resolved by the convention-based view locator bundled with this sample solution.
            // We just have to register the one here because it has a contract.
            Locator.CurrentMutable.Register(
                () => new DinosaurListItemView(),
                typeof(IViewFor<DinosaurViewModel>),
                "listitem");
        }

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