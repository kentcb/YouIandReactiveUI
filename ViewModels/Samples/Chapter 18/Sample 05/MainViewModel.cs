namespace Book.ViewModels.Samples.Chapter18.Sample05
{
    using System;
    using System.Reactive;
    using System.Reactive.Linq;
    using System.Text;
    using ReactiveUI;

    [Sample(
        "Custom Activation",
        @"This sample demonstrates the use of a custom implementation of `IActivationForViewFetcher`.

You can choose to load or unload a child view. However, you can also choose to force the child to remain loaded. This ability to prevent deactivation is provided by a custom implementation of `IActivationForViewFetcher` - it is not something ReactiveUI does out of the box.")]
    public sealed class MainViewModel : ReactiveObject
    {
        private readonly ChildViewModel singleChildInstance;
        private readonly ReactiveCommand<Unit, Unit> loadChildCommand;
        private readonly ReactiveCommand<Unit, Unit> unloadChildCommand;
        private readonly ObservableAsPropertyHelper<string> messages;
        private ChildViewModel child;
        private IDisposable forcedActivation;

        public MainViewModel()
        {
            this.singleChildInstance = new ChildViewModel();
            var childLoaded = this
                .WhenAnyValue(x => x.Child)
                .Select(x => x != null);
            this.loadChildCommand = ReactiveCommand.Create(
                () => { this.Child = this.singleChildInstance; },
                childLoaded.Select(x => !x));
            this.unloadChildCommand = ReactiveCommand.Create(
                () => { this.Child = null; },
                childLoaded);

            this.messages = this.singleChildInstance.Messages
                .Scan(new StringBuilder(), (sb, next) => sb.AppendLine(next))
                .Select(x => x.ToString())
                .ToProperty(this, x => x.Messages, scheduler: RxApp.MainThreadScheduler);
        }

        public ReactiveCommand<Unit, Unit> LoadChildCommand => this.loadChildCommand;

        public ReactiveCommand<Unit, Unit> UnloadChildCommand => this.unloadChildCommand;

        public ChildViewModel Child
        {
            get => this.child;
            set => this.RaiseAndSetIfChanged(ref this.child, value);
        }

        public IDisposable ForcedActivation
        {
            get => this.forcedActivation;
            set => this.RaiseAndSetIfChanged(ref this.forcedActivation, value);
        }

        public string Messages => this.messages.Value;
    }
}