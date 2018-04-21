namespace Book.Views.Samples.Chapter18.Sample06
{
    using System.Reactive.Disposables;
    using ReactiveUI;
    using ViewModels.Samples.Chapter18.Sample06;

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

                        this
                            .OneWayBind(this.ViewModel, x => x.TimeActivated, x => x.timeActivatedRun.Text)
                            .DisposeWith(disposables);
                    });
        }
    }
}