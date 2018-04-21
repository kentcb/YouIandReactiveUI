namespace Book.ViewModels.Samples.Chapter18.Sample02
{
    using System.Reactive;
    using System.Reactive.Linq;
    using ReactiveUI;

    [Sample(
        "View Activation: expensive resource",
        @"This sample demonstrates the use of `WhenActivated` to ensure a view only consumes resources when it is active.

Use the buttons to switch between child views. One consumes an expensive resource (CPU) where the other does not. Keep an eye on your CPU usage as you switch between the two.")]
    public sealed class MainViewModel : ReactiveObject
    {
        private readonly ChildViewModel child1;
        private readonly ChildViewModel child2;
        private readonly ReactiveCommand<Unit, Unit> selectChild1Command;
        private readonly ReactiveCommand<Unit, Unit> selectChild2Command;
        private ChildViewModel selectedChild;

        public MainViewModel()
        {
            this.child1 = new ChildViewModel(true);
            this.child2 = new ChildViewModel(false);

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
        }

        public ReactiveCommand<Unit, Unit> SelectChild1Command => this.selectChild1Command;

        public ReactiveCommand<Unit, Unit> SelectChild2Command => this.selectChild2Command;

        public ChildViewModel SelectedChild
        {
            get => this.selectedChild;
            set => this.RaiseAndSetIfChanged(ref this.selectedChild, value);
        }
    }
}