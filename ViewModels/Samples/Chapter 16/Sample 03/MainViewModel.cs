namespace Book.ViewModels.Samples.Chapter16.Sample03
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive.Linq;
    using ReactiveUI;

    [Sample(
        "Contract-based view selection",
        @"This sample demonstrates the use of contracts in conjunction with `ViewModelViewHost`.

The list of dinosaurs and the selected dinosaur are both surfaced as instances of `DinosaurViewModel`. Each item in the list is rendered via a `ViewModelViewHost`, as are the details when a dinosaur is selected. In order to display the same view model in different ways, a contract is specified to resolve a different view in each case.")]
    public sealed class MainViewModel : ReactiveObject
    {
        private readonly IList<DinosaurViewModel> dinosaurs;
        private DinosaurViewModel selectedDinosaur;

        public MainViewModel()
        {
            this.dinosaurs = Data
                .Dinosaurs
                .All
                .Where(dinosaur => dinosaur.Diet != null && dinosaur.ImageResourceName != null)
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