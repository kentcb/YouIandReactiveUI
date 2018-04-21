namespace Book.ViewModels.Samples.Chapter08.Sample02
{
    using System.Linq;
    using System.Reactive;
    using System.Reactive.Linq;
    using ReactiveUI;

    [Sample(
        "Synchronous Command with canExecute",
        @"This sample demonstrates the use of `ReactiveCommand` with a `canExecute` to create synchronous commands that can only be executed under certain scenarios.

Both buttons are backed by a `ReactiveCommand<Unit, Unit>`, and the logic for the buttons executes synchronously. Each button is constructed with a `canExecute` that dictates whether that command can execute. In the case of `AddDinosaurCommand`, it can only execute if a name has been entered. In the case of `DeleteDinosaurCommand`, it can only execute if a dinosaur has been selected in the list.")]
    public sealed class MainViewModel : ReactiveObject
    {
        private readonly IReactiveList<string> dinosaurs;
        private readonly ReactiveCommand<Unit, Unit> addDinosaurCommand;
        private readonly ReactiveCommand<Unit, Unit> deleteDinosaurCommand;
        private string name;
        private string selectedDinosaur;

        public MainViewModel()
        {
            this.dinosaurs = new ReactiveList<string>(
                Data
                    .Dinosaurs
                    .All
                    .Select(dinosaur => dinosaur.Name)
                    .ToList());

            var canAdd = this
                .WhenAnyValue(x => x.Name)
                .Select(name => !string.IsNullOrWhiteSpace(name));
            this.addDinosaurCommand = ReactiveCommand.Create(
                () => this.dinosaurs.Add(this.Name),
                canAdd);
            var canDelete = this
                .WhenAnyValue(x => x.SelectedDinosaur)
                .Select(selectedDinosaur => selectedDinosaur != null);
            this.deleteDinosaurCommand = ReactiveCommand.Create(
                () => { this.dinosaurs.Remove(this.SelectedDinosaur); },
                canDelete);
        }

        public IReactiveList<string> Dinosaurs => this.dinosaurs;

        public ReactiveCommand<Unit, Unit> AddDinosaurCommand => this.addDinosaurCommand;

        public ReactiveCommand<Unit, Unit> DeleteDinosaurCommand => this.deleteDinosaurCommand;

        public string Name
        {
            get => this.name;
            set => this.RaiseAndSetIfChanged(ref this.name, value);
        }

        public string SelectedDinosaur
        {
            get => this.selectedDinosaur;
            set => this.RaiseAndSetIfChanged(ref this.selectedDinosaur, value);
        }
    }
}
