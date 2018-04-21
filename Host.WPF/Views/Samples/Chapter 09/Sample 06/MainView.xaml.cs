namespace Book.Views.Samples.Chapter09.Sample06
{
    using System.Reactive;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using ReactiveUI;
    using ViewModels.Samples.Chapter09.Sample06;

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

                        this
                            .ViewModel
                            .ConfirmDeleteDinosaur
                            .RegisterHandler(
                                context =>
                                {
                                    undoView.ViewModel = context.Input;

                                    var undo = this
                                        .undoView
                                        .Undo
                                        .Do(
                                            _ =>
                                            {
                                                context.SetOutput(false);
                                                undoView.ViewModel = null;
                                            })
                                        .Select(_ => Unit.Default);

                                    var timedOut = context
                                        .Input
                                        .WhenAnyValue(x => x.TimeRemaining)
                                        .Where(timeRemaining => timeRemaining == 0)
                                        .FirstAsync()
                                        .Do(
                                            _ =>
                                            {
                                                context.SetOutput(true);
                                                undoView.ViewModel = null;
                                            })
                                        .Select(_ => Unit.Default);

                                    return Observable
                                        .Merge(
                                            undo,
                                            timedOut);
                                })
                            .DisposeWith(disposables);
                    });
        }
    }
}