namespace Book.ViewModels.Samples.Chapter06.Sample04
{
    using System.Linq;
    using System.Reactive;
    using System.Reactive.Linq;
    using ReactiveUI;

    [Sample(
        "WhenAnyValue with property chain",
        @"This sample demonstrates the use of `WhenAnyValue` to monitor property chains.

The properties for each dinosaur are stored in child view models. Those properties are monitored from the parent view model using `WhenAnyValue`.")]
    public sealed class MainViewModel : ReactiveObject
    {
        private readonly ReactiveCommand<Unit, Unit> resetDinosaurACommand;
        private readonly ReactiveCommand<Unit, Unit> resetDinosaurBCommand;
        private readonly ObservableAsPropertyHelper<string> winner;
        private DinosaurViewModel dinosaurA;
        private DinosaurViewModel dinosaurB;

        public MainViewModel()
        {
            this.dinosaurA = new DinosaurViewModel();
            this.dinosaurB = new DinosaurViewModel();

            this.resetDinosaurACommand = ReactiveCommand.Create(
                () =>
                {
                    this.DinosaurA = new DinosaurViewModel();
                });
            this.resetDinosaurBCommand = ReactiveCommand.Create(
                () =>
                {
                    this.DinosaurB = new DinosaurViewModel();
                });
            this.winner = this
                .WhenAnyValue(
                    x => x.DinosaurA.Weight,
                    x => x.DinosaurA.HasClub,
                    x => x.DinosaurA.HasHorns,
                    x => x.DinosaurA.HasScales,
                    x => x.DinosaurA.HasSpikes,
                    x => x.DinosaurB.Weight,
                    x => x.DinosaurB.HasClub,
                    x => x.DinosaurB.HasHorns,
                    x => x.DinosaurB.HasScales,
                    x => x.DinosaurB.HasSpikes,
                    (_0,_1,_2,_3,_4,_5,_6,_7,_8,_9) => this.dinosaurA.CompareTo(this.dinosaurB))
                .Select(
                    compareResult =>
                    {
                        if (compareResult == 0)
                        {
                            return "It's a tie.";
                        }
                        else if (compareResult > 0)
                        {
                            return "Dinosaur A wins!";
                        }
                        else
                        {
                            return "Dinosaur B wins!";
                        }
                    })
                .ToProperty(this, x => x.Winner);
        }

        public ReactiveCommand<Unit, Unit> ResetDinosaurACommand => this.resetDinosaurACommand;

        public ReactiveCommand<Unit, Unit> ResetDinosaurBCommand => this.resetDinosaurBCommand;

        public string Winner => this.winner.Value;

        public DinosaurViewModel DinosaurA
        {
            get => this.dinosaurA;
            private set => this.RaiseAndSetIfChanged(ref this.dinosaurA, value);
        }

        public DinosaurViewModel DinosaurB
        {
            get => this.dinosaurB;
            private set => this.RaiseAndSetIfChanged(ref this.dinosaurB, value);
        }
    }
}