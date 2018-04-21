namespace Book.ViewModels.Samples.Chapter18.Sample01
{
    using System.Reactive;
    using System.Reactive.Linq;
    using System.Text;
    using ReactiveUI;

    [Sample(
        "View Activation: simple",
        @"This sample demonstrates the use of `WhenActivated` in views.

Use the buttons to load one of two child views. Notice the events that occur as you switch between views.")]
    public sealed class MainViewModel : ReactiveObject
    {
        private readonly ChildViewModel child1;
        private readonly ChildViewModel child2;
        private readonly ReactiveCommand<Unit, Unit> selectChild1Command;
        private readonly ReactiveCommand<Unit, Unit> selectChild2Command;
        private readonly ObservableAsPropertyHelper<string> messages;
        private ChildViewModel selectedChild;

        public MainViewModel()
        {
            this.child1 = new ChildViewModel("Plesiosaurus");
            this.child2 = new ChildViewModel("Troodon");

            var child1Selectable = this
                .WhenAnyValue(x => x.SelectedChild)
                .Select(x => x == null || x == this.child2);
            var child2Selectable = this
                .WhenAnyValue(x => x.SelectedChild)
                .Select(x => x == null || x == this.child1);
            this.selectChild1Command = ReactiveCommand.Create(
                () => { this.SelectedChild = this.child1; },
                child1Selectable,
                RxApp.MainThreadScheduler);
            this.selectChild2Command = ReactiveCommand.Create(
                () => { this.SelectedChild = this.child2; },
                child2Selectable,
                RxApp.MainThreadScheduler);

            this.messages = this.child1.Messages
                .Merge(this.child2.Messages)
                .Scan(new StringBuilder(), (sb, next) => sb.AppendLine(next))
                .Select(x => x.ToString())
                .ToProperty(this, x => x.Messages, scheduler: RxApp.MainThreadScheduler);
        }

        public ReactiveCommand<Unit, Unit> SelectChild1Command => this.selectChild1Command;

        public ReactiveCommand<Unit, Unit> SelectChild2Command => this.selectChild2Command;

        public ChildViewModel SelectedChild
        {
            get => this.selectedChild;
            set => this.RaiseAndSetIfChanged(ref this.selectedChild, value);
        }

        public string Messages => this.messages.Value;
    }
}