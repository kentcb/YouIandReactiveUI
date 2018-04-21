namespace Book.ViewModels.Samples.Chapter09.Sample02
{
    using System;
    using System.Linq;
    using System.Reactive;
    using System.Reactive.Linq;
    using ReactiveUI;

    [Sample(
        "Interaction (TPL)",
        @"This sample demonstrates the use of `Interaction<TInput, TOutput>` to confirm deletion of dinosaurs with the user, with a TPL-based handler.

The `MainViewModel` declare an `Interaction<string, bool>`. When the user attempts to delete a dinosaur, it calls `Handle` against the interaction, passing in the name of the dinosaur that the user is attempting to delete.

The view handles the interaction by asking the user to confirm. It then calls `SetOutput` against the interaction's context object, telling the view model what answer the user gave.")]
    public sealed class MainViewModel : ReactiveObject
    {
        private readonly IReactiveList<string> dinosaurs;
        private readonly ReactiveCommand<Unit, Unit> addDinosaurCommand;
        private readonly ReactiveCommand<Unit, Unit> deleteDinosaurCommand;
        private readonly Interaction<string, bool> confirmDeleteDinosaur;
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

            this.confirmDeleteDinosaur = new Interaction<string, bool>();

            var canAdd = this
                .WhenAnyValue(x => x.Name)
                .Select(name => !string.IsNullOrWhiteSpace(name));
            this.addDinosaurCommand = ReactiveCommand.CreateFromObservable(
                () =>
                    Observable
                        .Return(Unit.Default)
                        .Delay(TimeSpan.FromSeconds(1), RxApp.MainThreadScheduler)
                        .Do(_ => this.dinosaurs.Add(this.Name)),
                canAdd);
            var canDelete = this
                .WhenAnyValue(x => x.SelectedDinosaur)
                .Select(selectedDinosaur => selectedDinosaur != null);
            this.deleteDinosaurCommand = ReactiveCommand.CreateFromObservable(
                () =>
                    this
                        .confirmDeleteDinosaur
                        .Handle(this.SelectedDinosaur)
                        .Where(result => result)
                        .Select(_ => Unit.Default)
                        .ObserveOn(RxApp.MainThreadScheduler)
                        .Do(_ => this.dinosaurs.Remove(this.SelectedDinosaur)),
                canDelete);
        }

        public IReactiveList<string> Dinosaurs => this.dinosaurs;

        public ReactiveCommand<Unit, Unit> AddDinosaurCommand => this.addDinosaurCommand;

        public ReactiveCommand<Unit, Unit> DeleteDinosaurCommand => this.deleteDinosaurCommand;

        public Interaction<string, bool> ConfirmDeleteDinosaur => this.confirmDeleteDinosaur;

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