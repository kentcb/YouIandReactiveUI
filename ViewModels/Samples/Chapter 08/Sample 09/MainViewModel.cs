namespace Book.ViewModels.Samples.Chapter08.Sample09
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive.Linq;
    using ReactiveUI;

    [Sample(
        "Parameters and Results",
        @"This sample demonstrates the use of a `ReactiveCommand` that takes a parameter and returns a result.

The view model exposes a single `LoadDinosaurs` command that accepts a letter range determining which dinosaurs to load. It returns the names of all relevant dinosaurs.")]
    public sealed class MainViewModel : ReactiveObject
    {
        private readonly ReactiveCommand<CharacterRange, IList<string>> loadDinosaursCommand;
        private readonly ObservableAsPropertyHelper<IList<string>> dinosaurs;

        public MainViewModel()
        {
            this.loadDinosaursCommand = ReactiveCommand.CreateFromObservable<CharacterRange, IList<string>>(
                this.LoadDinosaurs);

            this.dinosaurs = this
                .loadDinosaursCommand
                .ToProperty(this, x => x.Dinosaurs);
        }

        public ReactiveCommand<CharacterRange, IList<string>> LoadDinosaursCommand => this.loadDinosaursCommand;

        public IList<string> Dinosaurs => this.dinosaurs.Value;

        private IObservable<IList<string>> LoadDinosaurs(CharacterRange nameStartsWith) =>
            Observable
                .Return(
                    Data
                        .Dinosaurs
                        .All
                        .Where(dinosaur => nameStartsWith.Contains(dinosaur.Name[0]))
                        .Select(dinosaur => dinosaur.Name)
                        .OrderBy(x => x)
                        .ToList())
                .Delay(TimeSpan.FromMilliseconds(250));
    }

    public struct CharacterRange
    {
        private readonly char start;
        private readonly char end;

        public CharacterRange(
            char start,
            char end)
        {
            this.start = start;
            this.end = end;
        }

        public char Start => this.start;

        public char End => this.end;

        public bool Contains(char ch) =>
            ch >= this.start && ch <= this.end;
    }
}
