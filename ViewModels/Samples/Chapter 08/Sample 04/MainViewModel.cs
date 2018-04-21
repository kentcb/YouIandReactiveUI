namespace Book.ViewModels.Samples.Chapter08.Sample04
{
    using System;
    using System.Linq;
    using System.Reactive;
    using System.Reactive.Linq;
    using System.Threading.Tasks;
    using ReactiveUI;

    [Sample(
        "Asynchronous Command (TPL)",
        @"This sample demonstrates the use of `ReactiveCommand` to create asynchronous commands whose execution logic is represented by a `Task`.

Both buttons are backed by a `ReactiveCommand<Unit, Unit>`, and the logic for the buttons executes asynchronously (with a fake, one second delay).")]
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

            this.addDinosaurCommand = ReactiveCommand.CreateFromTask(
                async () =>
                {
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    this.dinosaurs.Remove(this.SelectedDinosaur);
                });
            this.deleteDinosaurCommand = ReactiveCommand.CreateFromTask(
                async () =>
                {
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    this.dinosaurs.Remove(this.SelectedDinosaur);
                });
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