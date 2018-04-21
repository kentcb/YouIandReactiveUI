namespace Book.ViewModels.Samples.Chapter16.Sample02
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive.Linq;
    using Book.ViewModels.Data;
    using ReactiveUI;

    [Sample(
        "Heterogeneous data",
        @"This sample demonstrates the use of `ViewModelViewHost` in resolving the correct view for the runtime type of a given view model.

The `MainViewModel`'s `SelectedDinosaur` property is of type `DinosaurViewModel`, which is an abstract type. Various subclasses of this type exist, and each needs to be displayed somewhat differently according to the dinosaur's diet (carnivore, herbivore, or omnivore). Each subclass has an associated view type, so assigning the view model to the `ViewModelViewHost.ViewModel` property results in the correct view being displayed.")]
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
                .Select<Dinosaur, DinosaurViewModel>(
                    dinosaur =>
                    {
                        switch (dinosaur.Diet.Value)
                        {
                            case Diet.Carnivore:
                                return new CarnivoreDinosaurViewModel(dinosaur);
                            case Diet.Herbivore:
                                return new HerbivoreDinosaurViewModel(dinosaur);
                            case Diet.Omnivore:
                                return new OmnivoreDinosaurViewModel(dinosaur);
                            default:
                                throw new NotSupportedException();
                        }
                    })
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