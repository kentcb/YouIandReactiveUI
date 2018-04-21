namespace Book.Views.Samples.Chapter18.Sample05
{
    using System;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Windows.Controls.Primitives;
    using ReactiveUI;
    using Splat;
    using ViewModels.Samples.Chapter18.Sample05;

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
                            .OneWayBind(this.ViewModel, x => x.Child, x => x.childViewModelViewHost.ViewModel)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.Messages, x => x.messagesTextBox.Text)
                            .DisposeWith(disposables);

                        this
                            .BindCommand(this.ViewModel, x => x.LoadChildCommand, x => x.loadChildButton)
                            .DisposeWith(disposables);
                        this
                            .BindCommand(this.ViewModel, x => x.UnloadChildCommand, x => x.unloadChildButton)
                            .DisposeWith(disposables);

                        var forcibleActivation = Locator.Current.GetService<IForcibleActivationForViewFetcher>();
                        var forcedActivation = new SerialDisposable();

                        this
                            .WhenAnyValue(x => x.ViewModel.ForcedActivation)
                            .Subscribe(x => forcedActivation.Disposable = x)
                            .DisposeWith(disposables);

                        this
                            .WhenAnyValue(x => x.ViewModel.Child, x => x.ViewModel.ForcedActivation, (child, activation) => child != null || activation != null)
                            .BindTo(this.forceActivationCheckBox, x => x.IsEnabled)
                            .DisposeWith(disposables);

                        this
                            .forceActivationCheckBox
                            .Events()
                            .Checked
                            .Select(_ => this.childViewModelViewHost.Content as IActivatable)
                            .Select(x => x != null ? forcibleActivation.Force(x) : null)
                            .Subscribe(x => this.ViewModel.ForcedActivation = x)
                            .DisposeWith(disposables);

                        this
                            .forceActivationCheckBox
                            .Events()
                            .Unchecked
                            .Subscribe(x => this.ViewModel.ForcedActivation = null)
                            .DisposeWith(disposables);
                    });
        }
    }
}