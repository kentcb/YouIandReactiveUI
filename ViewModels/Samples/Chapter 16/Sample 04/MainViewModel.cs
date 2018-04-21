namespace Book.ViewModels.Samples.Chapter16.Sample04
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive.Linq;
    using ReactiveUI;

    [Sample(
        "Default content",
        @"This sample demonstrates the use of default content when using `ViewModelViewHost`.

The `ViewModelViewHost` displaying the details of the selected dinosaur specifies ")]
    public sealed class MainViewModel : ReactiveObject
    {
        private readonly IList<DinosaurViewModel> dinosaurs;
        private DinosaurViewModel selectedDinosaur;

        public MainViewModel()
        {
            this.dinosaurs = Data
                .Dinosaurs
                .All
                .Where(dinosaur => dinosaur.Diet != null)
                .Select(dinosaur => new DinosaurViewModel(dinosaur))
                .OrderBy(dinosaur => dinosaur.Name)
                .ToList();
        }

        public IList<DinosaurViewModel> Dinosaurs => this.dinosaurs;

        public DinosaurViewModel SelectedDinosaur
        {
            get => this.selectedDinosaur;
            set => this.RaiseAndSetIfChanged(ref this.selectedDinosaur, value);
        }
    }
}