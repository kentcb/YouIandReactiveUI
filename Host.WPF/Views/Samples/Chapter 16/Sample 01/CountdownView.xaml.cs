namespace Book.Views.Samples.Chapter16.Sample01
{
    using System.Reactive.Disposables;
    using ReactiveUI;
    using ViewModels.Samples.Chapter16.Sample01;

    public partial class CountdownView : ReactiveUserControl<CountdownViewModel>
    {
        public CountdownView()
        {
            InitializeComponent();

            this
                .WhenActivated(
                    disposables =>
                    {
                        this
                            .OneWayBind(this.ViewModel, x => x.TimeRemaining, x => x.timeRemainingLabel.Content)
                            .DisposeWith(disposables);
                    });
        }
    }
}