namespace Book.ViewModels.Samples.Chapter04.Sample04
{
    using System;
    using ReactiveUI;

    public sealed class DinosaurExhibitionViewModel : ReactiveObject
    {
        private readonly string what;
        private readonly DateTimeOffset when;
        private readonly string where;

        public DinosaurExhibitionViewModel(
            IDinosaurExhibition dinosaurExhibition)
        {
            this.what = dinosaurExhibition.What;
            this.when = dinosaurExhibition.When;
            this.where = dinosaurExhibition.Where;
        }

        public string What => this.what;

        public DateTimeOffset When => this.when;

        public string WhenDisplay => this.When.ToString("f");

        public string Where => this.where;
    }
}