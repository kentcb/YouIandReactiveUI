namespace Book.ViewModels.Samples.Chapter06.Sample04
{
    using System;
    using ReactiveUI;

    public sealed class DinosaurViewModel : ReactiveObject, IComparable<DinosaurViewModel>
    {
        private decimal? weight;
        private bool hasScales;
        private bool hasHorns;
        private bool hasSpikes;
        private bool hasClub;

        public decimal? Weight
        {
            get => this.weight;
            set => this.RaiseAndSetIfChanged(ref this.weight, value);
        }

        public bool HasScales
        {
            get => this.hasScales;
            set => this.RaiseAndSetIfChanged(ref this.hasScales, value);
        }

        public bool HasHorns
        {
            get => this.hasHorns;
            set => this.RaiseAndSetIfChanged(ref this.hasHorns, value);
        }

        public bool HasSpikes
        {
            get => this.hasSpikes;
            set => this.RaiseAndSetIfChanged(ref this.hasSpikes, value);
        }

        public bool HasClub
        {
            get => this.hasClub;
            set => this.RaiseAndSetIfChanged(ref this.hasClub, value);
        }

        public int CompareTo(DinosaurViewModel other)
        {
            if (other == null)
            {
                return 1;
            }

            return GetCharacterPoints(this).CompareTo(GetCharacterPoints(other));
        }

        private static int GetCharacterPoints(DinosaurViewModel dinosaur) =>
            // my super-scientific Dino Character Point Calculator™ ©
            (int)dinosaur.weight.GetValueOrDefault() +
            (dinosaur.hasScales ? 100 : 0) +
            (dinosaur.HasHorns ? 120 : 0) +
            (dinosaur.HasSpikes ? 80 : 0) +
            (dinosaur.HasClub ? 90 : 0);
    }
}