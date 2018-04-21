namespace Book.ViewModels.Samples.Chapter17.Sample02
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive.Linq;
    using ReactiveUI;

    public sealed class ScientistsViewModel : ReactiveObject, IRoutableViewModel
    {
        private readonly IScreen hostScreen;
        private readonly IList<ScientistViewModel> scientists;
        private ScientistViewModel selectedScientist;

        public ScientistsViewModel(IScreen hostScreen)
        {
            this.hostScreen = hostScreen;
            this.scientists = Data
                .Scientists
                .All
                .Select(scientist => new ScientistViewModel(scientist, this.hostScreen))
                .ToList();

            this
                .WhenAnyValue(x => x.SelectedScientist)
                .Where(selectedScientist => selectedScientist != null)
                .Do(_ => this.SelectedScientist = null)
                .SelectMany(selectedScientist => this.hostScreen.Router.Navigate.Execute(selectedScientist))
                .Subscribe();
        }

        public string UrlPathSegment => "Scientists";

        public IScreen HostScreen => this.hostScreen;

        public IList<ScientistViewModel> Scientists => this.scientists;

        public ScientistViewModel SelectedScientist
        {
            get => this.selectedScientist;
            set => this.RaiseAndSetIfChanged(ref this.selectedScientist, value);
        }
    }
}