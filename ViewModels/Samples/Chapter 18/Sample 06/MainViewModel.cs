namespace Book.ViewModels.Samples.Chapter18.Sample06
{
    using System.Reactive;
    using System.Reactive.Linq;
    using System.Text;
    using ReactiveUI;

    [Sample(
        "Activation in Pipelines",
        @"This sample demonstrates the use of extension methods (contained in `GetIsActivatedExtensions`) to turn an activatable object into an `IObservable<bool>`.

Below you can load or unload a child view. The child view's view model executes a timer, but it only does so while activated. You can see this because unloading the view stops the timer from ticking.

Importantly, `ChildViewModel` incorporates activation into a pipeline by calling `GetIsActivated` against itself. It can therefore use the activation pipeline to construct an `ObservableAsPropertyHelper<bool>` during construction.")]
    public sealed class MainViewModel : ReactiveObject
    {
        private readonly ChildViewModel childInstance;
        private readonly ReactiveCommand<Unit, Unit> loadChildCommand;
        private readonly ReactiveCommand<Unit, Unit> unloadChildCommand;
        private readonly ObservableAsPropertyHelper<string> messages;
        private ChildViewModel child;

        public MainViewModel()
        {
            this.childInstance = new ChildViewModel();

            var childLoaded = this
                .WhenAnyValue(x => x.Child)
                .Select(x => x != null);
            this.loadChildCommand = ReactiveCommand.Create(
                () => { this.Child = this.childInstance; },
                childLoaded.Select(x => !x));
            this.unloadChildCommand = ReactiveCommand.Create(
                () => { this.Child = null; },
                childLoaded);

            this.messages = this
                .childInstance
                .Messages
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

        public string Messages => this.messages.Value;
    }
}