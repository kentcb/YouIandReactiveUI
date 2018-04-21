namespace Book.ViewModels.Samples.Chapter08.Sample08
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive;
    using System.Reactive.Linq;
    using Data;
    using ReactiveUI;

    [Sample(
        "Combined Command with canExecute",
        @"This sample demonstrates the use of `ReactiveCommand` to create combined commands that also honor an additional `canExecute` observable.

`RemoveErasCommand` will remove any era names from the given body of text. It can only execute if the text contains at least one era name. Similarly, `RemovePeriodsCommand` does the same for periods. `RemoveBothCommand` is a `CombinedReactiveCommand` that executes both commands. It can only execute if both commands can also execute, but also requires that `IsConfirmed` be `true`.

If you're stuck trying to figure out valid era names, try **Mesozoic** or **Cenozoic**. For periods, try **Cambrian**, **Permian**, or **Jurassic**.")]
    public sealed class MainViewModel : ReactiveObject
    {
        private static readonly IList<Era> eras = Enum
            .GetValues(typeof(Era))
            .Cast<Era>()
            .ToList();
        private static readonly IList<Period> periods = Enum
            .GetValues(typeof(Period))
            .Cast<Period>()
            .ToList();

        private readonly ReactiveCommand<Unit, Unit> removeErasCommand;
        private readonly ReactiveCommand<Unit, Unit> removePeriodsCommand;
        private readonly CombinedReactiveCommand<Unit, Unit> removeBothCommand;
        private string text;
        private bool isConfirmed;

        public MainViewModel()
        {
            this.text = @"The Plesiosaurus lived during the Mesozoic era, in the Jurassic period. Its named means 'near lizard', derived from the Greek words plesios (near to) and sauros (lizard). The first Plesiosaurus fossils were found in England by Mary Anning in 1821.

The Eryops was a primitive amphibian alive during the Permian period (Palaeozoic era) around 270 million years ago. It was part of the Eryopidae family. Its named means 'drawn-out face' due to the fact that most of its skull is in front of its eyes.";

            var canRemoveEras = this
                .WhenAnyValue(x => x.Text)
                .Select(text => text != null && eras.Any(era => text.Contains(era.ToString())));
            this.removeErasCommand = ReactiveCommand.Create(this.RemoveEras, canRemoveEras);

            var canRemovePeriods = this
                .WhenAnyValue(x => x.Text)
                .Select(text => text != null && periods.Any(period => text.Contains(period.ToString())));
            this.removePeriodsCommand = ReactiveCommand.Create(this.RemovePeriods, canRemovePeriods);

            this.removeBothCommand = ReactiveCommand.CreateCombined(
                new[]
                {
                    this.removeErasCommand,
                    this.removePeriodsCommand
                },
                this.WhenAnyValue(x => x.IsConfirmed));
        }

        public ReactiveCommand<Unit, Unit> RemoveErasCommand => this.removeErasCommand;

        public ReactiveCommand<Unit, Unit> RemovePeriodsCommand => this.removePeriodsCommand;

        public CombinedReactiveCommand<Unit, Unit> RemoveBothCommand => this.removeBothCommand;

        public string Text
        {
            get => this.text;
            set => this.RaiseAndSetIfChanged(ref this.text, value);
        }

        public bool IsConfirmed
        {
            get => this.isConfirmed;
            set => this.RaiseAndSetIfChanged(ref this.isConfirmed, value);
        }

        private void RemoveEras()
        {
            var replacementText = eras
                .Aggregate(this.Text, (acc, next) => acc.Replace(next.ToString(), ""));
            this.Text = replacementText;
        }

        private void RemovePeriods()
        {
            var replacementText = periods
                .Aggregate(this.Text, (acc, next) => acc.Replace(next.ToString(), ""));
            this.Text = replacementText;
        }
    }
}
