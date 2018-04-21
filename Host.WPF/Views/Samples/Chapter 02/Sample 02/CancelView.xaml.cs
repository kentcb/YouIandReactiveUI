namespace Book.Views.Samples.Chapter02.Sample02
{
    using System;
    using System.Reactive;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Windows;
    using System.Windows.Controls.Primitives;
    using ReactiveUI;
    using ViewModels.Samples.Chapter02.Sample02;

    public partial class CancelView : ReactiveUserControl<CancelViewModel>
    {
        public CancelView()
        {
            InitializeComponent();

            this
                .WhenActivated(
                    disposables =>
                    {
                        this
                            .OneWayBind(this.ViewModel, x => x.TimeRemaining, x => x.cancelButton.Content, timeRemaining => $"Cancel ({timeRemaining})")
                            .DisposeWith(disposables);

                        this
                            .WhenAnyValue(x => x.ViewModel)
                            .Select(viewModel => viewModel == null ? Visibility.Collapsed : Visibility.Visible)
                            .BindTo(this, x => x.Visibility)
                            .DisposeWith(disposables);
                    });
        }

        public IObservable<Unit> Cancel =>
            this
                .cancelButton
                .Events()
                .Click
                .Select(_ => Unit.Default);
    }
}