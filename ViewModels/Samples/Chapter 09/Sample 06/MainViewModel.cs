namespace Book.ViewModels.Samples.Chapter09.Sample06
{
    using System;
    using System.Linq;
    using System.Reactive;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using ReactiveUI;

    [Sample(
        "Undo interaction",
        @"This sample demonstrates using an interaction to facilitate a timed undo toast.

When the user deletes a dinosaur, the deletion is enacted immediately. However, an interaction is also started to give the user a small window of time via which they can undo the deletion.")]
    public sealed class MainViewModel : ReactiveObject
    {
        private readonly IReactiveList<string> dinosaurs;
        private readonly ReactiveCommand<Unit, Unit> addDinosaurCommand;
        private readonly ReactiveCommand<Unit, Unit> deleteDinosaurCommand;
        private readonly Interaction<UndoViewModel, bool> confirmDeleteDinosaur;
        private readonly SerialDisposable outstandingUndoInteraction;
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

            this.confirmDeleteDinosaur = new Interaction<UndoViewModel, bool>();
            this.outstandingUndoInteraction = new SerialDisposable();

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
            this.deleteDinosaurCommand = ReactiveCommand.Create(
                () =>
                {
                    var index = this.dinosaurs.IndexOf(this.SelectedDinosaur);
                    var dinosaurName = this.dinosaurs[index];
                    this.dinosaurs.RemoveAt(index);
                    var undoInfo = new UndoViewModel(dinosaurName);

                    this.outstandingUndoInteraction.Disposable = this
                        .confirmDeleteDinosaur
                        .Handle(undoInfo)
                        .Where(answer => !answer)
                        .Do(_ => this.dinosaurs.Insert(index, dinosaurName))
                        .Subscribe();
                },
                canDelete);
        }

        public IReactiveList<string> Dinosaurs => this.dinosaurs;

        public ReactiveCommand<Unit, Unit> AddDinosaurCommand => this.addDinosaurCommand;

        public ReactiveCommand<Unit, Unit> DeleteDinosaurCommand => this.deleteDinosaurCommand;

        public Interaction<UndoViewModel, bool> ConfirmDeleteDinosaur => this.confirmDeleteDinosaur;

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