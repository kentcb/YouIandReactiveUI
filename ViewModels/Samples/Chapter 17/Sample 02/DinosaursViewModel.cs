namespace Book.ViewModels.Samples.Chapter17.Sample02
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive.Linq;
    using ReactiveUI;

    public sealed class DinosaursViewModel : ReactiveObject, IRoutableViewModel
    {
        private readonly IScreen hostScreen;
        private readonly IList<DinosaurViewModel> dinosaurs;
        private DinosaurViewModel selectedDinosaur;

        public DinosaursViewModel(IScreen hostScreen)
        {
            this.hostScreen = hostScreen;
            this.dinosaurs = Data
                .Dinosaurs
                .All
                .Where(dinosaur => dinosaur.ImageResourceName != null)
                .Select(dinosaur => new DinosaurViewModel(dinosaur, this.hostScreen))
                .ToList();

            this
                .WhenAnyValue(x => x.SelectedDinosaur)
                .Where(selectedDinosaur => selectedDinosaur != null)
                .Do(_ => this.SelectedDinosaur = null)
                .SelectMany(selectedDinosaur => this.hostScreen.Router.Navigate.Execute(selectedDinosaur))
                .Subscribe();
        }

        public string UrlPathSegment => "Dinosaurs";

        public IScreen HostScreen => this.hostScreen;

        public IList<DinosaurViewModel> Dinosaurs => this.dinosaurs;

        public DinosaurViewModel SelectedDinosaur
        {
            get => this.selectedDinosaur;
            set => this.RaiseAndSetIfChanged(ref this.selectedDinosaur, value);
        }
    }
}