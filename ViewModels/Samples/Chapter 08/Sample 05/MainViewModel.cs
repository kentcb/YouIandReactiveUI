namespace Book.ViewModels.Samples.Chapter08.Sample05
{
    using System;
    using System.Linq;
    using System.Reactive;
    using System.Reactive.Linq;
    using ReactiveUI;

    [Sample(
        "Asynchronous Command with canExecute",
        @"This sample demonstrates the use of `ReactiveCommand` to create asynchronous commands that can only be executed under certain scenarios.

Both buttons are backed by a `ReactiveCommand<Unit, Unit>`, and the logic for the buttons executes asynchronously (with a fake delay). Each button is constructed with a `canExecute` that dictates whether that command can execute. In the case of `AddDinosaurCommand`, it can only execute if a name has been entered. In the case of `DeleteDinosaurCommand`, it can only execute if a dinosaur has been selected in the list.")]
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
                    Observable
                        .Return(Unit.Default)
                        .Delay(TimeSpan.FromSeconds(1), RxApp.MainThreadScheduler)
                        .Do(_ => this.dinosaurs.Remove(this.SelectedDinosaur)),
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
