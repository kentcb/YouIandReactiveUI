namespace Book.Views.Samples.Chapter09.Sample04
{
    using System.Reactive;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using ReactiveUI;
    using ViewModels.Samples.Chapter09.Sample04;

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
                            .Bind(this.ViewModel, x => x.Child, x => x.viewModelViewHost.ViewModel)
                            .DisposeWith(disposables);

                        SharedInteractions
                            .UnhandledException
                            .RegisterHandler(
                                context =>
                                    this
                                        .ShowMessage("Unexpected Error", "Sorry, we've run into unexpected problems while processing your request. Apologies for the inconvenience.")
                                        .Do(_ => context.SetOutput(Unit.Default)))
                            .DisposeWith(disposables);
                    });
        }
    }
}