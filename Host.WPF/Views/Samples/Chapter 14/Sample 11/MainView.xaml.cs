namespace Book.Views.Samples.Chapter14.Sample11
{
    using System;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Windows.Controls.Primitives;
    using ReactiveUI;
    using ViewModels.Samples.Chapter14.Sample11;

    public partial class MainView : ReactiveUserControl<MainViewModel>
    {
        public MainView()
        {
            InitializeComponent();

            this
                .WhenActivated(
                    disposables =>
                    {
                        var updateSignal = this
                            .nameTextBox
                            .Events()
                            .KeyUp
                            .Throttle(TimeSpan.FromSeconds(1), scheduler: RxApp.MainThreadScheduler);

                        this
                            .Bind(
                                this.ViewModel,
                                x => x.Name,
                                x => x.nameTextBox.Text,
                                signalViewUpdate: updateSignal)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.Display, x => x.displayLabel.Content)
                            .DisposeWith(disposables);
                    });
        }
    }
}