namespace Book.ViewModels.Samples.Chapter09.Sample05
{
    using System;
    using System.Linq;
    using System.Reactive;
    using System.Reactive.Linq;
    using ReactiveUI;

    [Sample(
        "'Yes to all' interaction",
        @"This sample demonstrates using an interaction where the user can opt to apply a given choice to future interactions.

Drawing cards will prompt the user to confirm each card drawn. When the user chooses **Yes to All** or **No to All**, the view stores off this decision so that it can answer future interactions with the same response. The view model is completely oblivious to the fact that the view might be responding to interactions without bothering the user.")]
    public sealed class MainViewModel : ReactiveObject
    {
        private static readonly Random random = new Random();
        private static readonly string[] suites = new[]
        {
            "Clubs",
            "Diamonds",
            "Hearts",
            "Spades"
        };

        private readonly Interaction<Unit, bool> drawAnotherInteraction;
        private readonly ReactiveCommand<Unit, Unit> draw1Command;
        private readonly ReactiveCommand<Unit, Unit> draw10Command;
        private readonly ReactiveList<string> hand;
        private readonly ObservableAsPropertyHelper<string> handDisplay;

        public MainViewModel()
        {
            this.drawAnotherInteraction = new Interaction<Unit, bool>();
            this.draw1Command = ReactiveCommand.CreateFromObservable(
                () => this.Draw(1));
            this.draw10Command = ReactiveCommand.CreateFromObservable(
                () => this.Draw(10));
            this.hand = new ReactiveList<string>();
            this.handDisplay = hand
                .Changed
                .Select(
                    _ =>
                        this
                            .hand
                            .Aggregate("You drew:", (acc, next) => acc + Environment.NewLine + next))
                .ToProperty(this, x => x.HandDisplay);
        }

        public Interaction<Unit, bool> DrawAnotherInteraction => this.drawAnotherInteraction;

        public ReactiveCommand<Unit, Unit> Draw1Command => this.draw1Command;

        public ReactiveCommand<Unit, Unit> Draw10Command => this.draw10Command;

        public string HandDisplay => this.handDisplay.Value;

        private IObservable<Unit> Draw(int number)
        {
            this.hand.Clear();

            return Observable
                .Defer(
                    () =>
                        this
                            .drawAnotherInteraction
                            .Handle(Unit.Default)
                            .Where(draw => draw)
                            .ObserveOn(RxApp.MainThreadScheduler)
                            .Select(_ => DrawRandomCard())
                            .Do(card => this.hand.Add(card)))
                .Repeat(number)
                .Aggregate(Unit.Default, (_, __) => Unit.Default);
        }

        private static string DrawRandomCard()
        {
            var dinosaurs = Data
                .Dinosaurs
                .All;
            var value = dinosaurs[random.Next(0, dinosaurs.Count)];
            var suite = suites[random.Next(0, suites.Length)];

            return $"{value.Name} of {suite}";
        }
    }
}