namespace Book.ViewModels.Samples.Chapter04.Sample04
{
    using System.Collections.Generic;
    using System.Linq;
    using global::Splat;
    using ReactiveUI;

    public sealed class ListExhibitionsViewModel : ReactiveObject
    {
        private IList<DinosaurExhibitionViewModel> exhibitions;

        public ListExhibitionsViewModel()
        {
            this.RepopulateExhibitions();

            Locator
                .CurrentMutable
                .ServiceRegistrationCallback(
                    typeof(IDinosaurExhibition),
                    _ => this.RepopulateExhibitions());
        }

        public IList<DinosaurExhibitionViewModel> Exhibitions
        {
            get => this.exhibitions;
            private set => this.RaiseAndSetIfChanged(ref this.exhibitions, value);
        }

        private void RepopulateExhibitions() =>
            this.Exhibitions = Locator
                .Current
                .GetServices<IDinosaurExhibition>()
                .Select(dinosaurExhibition => new DinosaurExhibitionViewModel(dinosaurExhibition))
                .ToList();
    }
}