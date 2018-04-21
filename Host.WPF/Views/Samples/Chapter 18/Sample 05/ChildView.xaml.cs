namespace Book.Views.Samples.Chapter18.Sample05
{
    using System.Reactive.Disposables;
    using ReactiveUI;
    using ViewModels.Samples.Chapter18.Sample05;

    public partial class ChildView : ReactiveUserControl<ChildViewModel>
    {
        public ChildView()
        {
            InitializeComponent();

            this
                .WhenActivated(
                    disposables =>
                    {
                        this.ViewModel?.LogMessage("Activated");
                        Disposable
                            .Create(() => this.ViewModel?.LogMessage("Deactivated"))
                            .DisposeWith(disposables);
                    });
        }
    }
}