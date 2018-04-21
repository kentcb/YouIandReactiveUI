namespace Book.ViewModels.Samples.Chapter14.Sample06
{
    using System.Reactive;
    using System.Reactive.Linq;
    using ReactiveUI;

    [Sample(
        "Bind: simple",
        @"This sample demonstrates the use of `Bind` by binding a `Name` property in the view model to an appropriate control in the view. Changes made by the user are automatically propagated to the view model. In addition, the user can clear the name via a command, which sets the `Name` property back to `null`. This shows that changes in the view model are also propagated to the view.")]
    public sealed class MainViewModel : ReactiveObject
    {
        private readonly ReactiveCommand<Unit, Unit> clearNameCommand;
        private readonly ObservableAsPropertyHelper<string> display;
        private string name;

        public MainViewModel()
        {
            this.clearNameCommand = ReactiveCommand.Create(() => { this.Name = null; });
            this.display = this
                .WhenAnyValue(x => x.Name)
                .Select(name => string.IsNullOrWhiteSpace(name) ? "Your dinosaur does not yet have a name." : $"Your dinosaur is named '{name}'.")
                .ToProperty(this, x => x.Display);
        }

        public ReactiveCommand<Unit, Unit> ClearNameCommand => this.clearNameCommand;

        public string Name
        {
            get => this.name;
            set => this.RaiseAndSetIfChanged(ref this.name, value);
        }

        public string Display => this.display.Value;
    }
}