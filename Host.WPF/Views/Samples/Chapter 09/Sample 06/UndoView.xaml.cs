namespace Book.Views.Samples.Chapter09.Sample06
{
    using System;
    using System.Reactive;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Windows;
    using System.Windows.Controls.Primitives;
    using ReactiveUI;
    using ViewModels.Samples.Chapter09.Sample06;

    public partial class UndoView : ReactiveUserControl<UndoViewModel>
    {
        public UndoView()
        {
            InitializeComponent();

            this
                .WhenActivated(
                    disposables =>
                    {
                        this
                            .OneWayBind(this.ViewModel, x => x.Name, x => x.confirmDeleteLabel.Content, name => $"Deleted '{name}'.")
                            .DisposeWith(disposables);

                        this
                            .OneWayBind(this.ViewModel, x => x.TimeRemaining, x => x.undoButton.Content, timeRemaining => $"Undo ({timeRemaining})")
                            .DisposeWith(disposables);

                        this
                            .WhenAnyValue(x => x.ViewModel)
                            .Select(viewModel => viewModel == null ? Visibility.Collapsed : Visibility.Visible)
                            .BindTo(this, x => x.Visibility)
                            .DisposeWith(disposables);
                    });
        }

        public IObservable<Unit> Undo =>
            this
                .undoButton
                .Events()
                .Click
                .Select(_ => Unit.Default);
    }
}