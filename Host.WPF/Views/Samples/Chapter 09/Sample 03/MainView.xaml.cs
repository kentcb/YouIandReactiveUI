namespace Book.Views.Samples.Chapter09.Sample03
{
    using System;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using ReactiveUI;
    using ViewModels.Samples.Chapter09.Sample03;

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
                            .Bind(this.ViewModel, x => x.DonGlasses, x => x.donGlassesCheckBoxes.IsChecked)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.SpottedDinosaur, x => x.spottedDinosaurLabel.Content)
                            .DisposeWith(disposables);
                        this
                            .BindCommand(this.ViewModel, x => x.SpotDinosaurCommand, x => x.spotDinosaurButton)
                            .DisposeWith(disposables);

                        this
                            .ViewModel
                            .SpotDinosaurInteraction
                            .RegisterHandler(
                                context =>
                                    context.SetOutput("Sorry, can't see any dinosaurs. Maybe try putting your glasses on?"))
                            .DisposeWith(disposables);

                        var specificHandler = new SerialDisposable()
                            .DisposeWith(disposables);
                        this
                            .WhenAnyValue(x => x.ViewModel.DonGlasses)
                            .Select(
                                donGlasses =>
                                    donGlasses ?
                                        this
                                            .ViewModel
                                            .SpotDinosaurInteraction
                                            .RegisterHandler(context => context.SetOutput("OMG, A T-REX!!")) :
                                        null)
                            .Do(disposable => specificHandler.Disposable = disposable)
                            .Subscribe()
                            .DisposeWith(disposables);
                    });
        }
    }
}