namespace Book.Views.Samples.Chapter18.Sample01
{
    using System;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Windows;
    using ReactiveUI;
    using ViewModels.Samples.Chapter18.Sample01;

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

                        this
                            .OneWayBind(this.ViewModel, x => x.Name, x => x.nameLabel.Content)
                            .DisposeWith(disposables);
                        Disposable
                            .Create(() => this.ViewModel?.LogMessage("Deactivated"))
                            .DisposeWith(disposables);
                    });

            this
                .Events()
                .Loaded
                .Do(_ => this.ViewModel?.LogMessage("Loaded"))
                .Subscribe();

            this
                .Events()
                .Unloaded
                .Do(_ => this.ViewModel?.LogMessage("Unloaded"))
                .Subscribe();
        }
    }
}