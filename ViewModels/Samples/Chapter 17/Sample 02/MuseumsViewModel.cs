namespace Book.ViewModels.Samples.Chapter17.Sample02
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive.Linq;
    using ReactiveUI;

    public sealed class MuseumsViewModel : ReactiveObject, IRoutableViewModel
    {
        private readonly IScreen hostScreen;
        private readonly IList<MuseumViewModel> museums;
        private MuseumViewModel selectedMuseum;

        public MuseumsViewModel(IScreen hostScreen)
        {
            this.hostScreen = hostScreen;
            this.museums = Data
                .Museums
                .All
                .Select(museum => new MuseumViewModel(museum, this.hostScreen))
                .ToList();

            this
                .WhenAnyValue(x => x.SelectedMuseum)
                .Where(selectedMuseum => selectedMuseum != null)
                .Do(_ => this.SelectedMuseum = null)
                .SelectMany(selectedMuseum => this.hostScreen.Router.Navigate.Execute(selectedMuseum))
                .Subscribe();
        }

        public string UrlPathSegment => "Museums";

        public IScreen HostScreen => this.hostScreen;

        public IList<MuseumViewModel> Museums => this.museums;

        public MuseumViewModel SelectedMuseum
        {
            get => this.selectedMuseum;
            set => this.RaiseAndSetIfChanged(ref this.selectedMuseum, value);
        }
    }
}