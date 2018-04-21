namespace Book.ViewModels.Samples.Chapter18.Sample04
{
    using System.Reactive;
    using System.Reactive.Linq;
    using System.Text;
    using ReactiveUI;

    [Sample(
        "View Model Activation: shared view model",
        @"This sample demonstrates the use of `WhenActivated` from a view model that is shared by multiple views.

Using the buttons you can choose to load or unload one of two views, both of which share the same `ChildViewModel`. Notice that the view model only deactivates when both views are unloaded.")]
    public sealed class MainViewModel : ReactiveObject
    {
        private readonly ChildViewModel singleChildInstance;
        private readonly ReactiveCommand<Unit, Unit> loadLeftChildCommand;
        private readonly ReactiveCommand<Unit, Unit> unloadLeftChildCommand;
        private readonly ReactiveCommand<Unit, Unit> loadRightChildCommand;
        private readonly ReactiveCommand<Unit, Unit> unloadRightChildCommand;
        private readonly ObservableAsPropertyHelper<string> messages;
        private ChildViewModel leftChild;
        private ChildViewModel rightChild;

        public MainViewModel()
        {
            this.singleChildInstance = new ChildViewModel();

            var leftChildLoaded = this
                .WhenAnyValue(x => x.LeftChild)
                .Select(x => x != null);
            var rightChildLoaded = this
                .WhenAnyValue(x => x.RightChild)
                .Select(x => x != null);
            this.loadLeftChildCommand = ReactiveCommand.Create(
                () => { this.LeftChild = this.singleChildInstance; },
                leftChildLoaded.Select(x => !x));
            this.unloadLeftChildCommand = ReactiveCommand.Create(
                () => { this.LeftChild = null; },
                leftChildLoaded);
            this.loadRightChildCommand = ReactiveCommand.Create(
                () => { this.RightChild = this.singleChildInstance; },
                rightChildLoaded.Select(x => !x));
            this.unloadRightChildCommand = ReactiveCommand.Create(
                () => { this.RightChild = null; },
                rightChildLoaded);

            this.messages = this.singleChildInstance.Messages
                .Scan(new StringBuilder(), (sb, next) => sb.AppendLine(next))
                .Select(x => x.ToString())
                .ToProperty(this, x => x.Messages, scheduler: RxApp.MainThreadScheduler);
        }

        public ReactiveCommand<Unit, Unit> LoadLeftChildCommand => this.loadLeftChildCommand;

        public ReactiveCommand<Unit, Unit> UnloadLeftChildCommand => this.unloadLeftChildCommand;

        public ReactiveCommand<Unit, Unit> LoadRightChildCommand => this.loadRightChildCommand;

        public ReactiveCommand<Unit, Unit> UnloadRightChildCommand => this.unloadRightChildCommand;

        public ChildViewModel LeftChild
        {
            get => this.leftChild;
            private set => this.RaiseAndSetIfChanged(ref this.leftChild, value);
        }

        public ChildViewModel RightChild
        {
            get => this.rightChild;
            private set => this.RaiseAndSetIfChanged(ref this.rightChild, value);
        }

        public string Messages => this.messages.Value;
    }
}