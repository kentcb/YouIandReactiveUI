namespace Book.ViewModels.Samples.Chapter10.Sample02
{
    using System;
    using System.Linq;
    using System.Reactive.Linq;
    using ReactiveUI;

    [Sample(
        "Item change notifications",
        @"This sample demonstrates the use of `ReactiveList<T>` to ensure the UI stays up to date when items themselves are modified.

The `MainViewModel` exposes a list of `DinosaurViewModel`. Each `DinosaurViewModel` has a `FossilCount` property, which the user can modify. Whenever a modification is made, the `MainViewModel` can detect it via `ItemChanged` and update its `TotalFossilCount` accordingly.")]
    public sealed class MainViewModel : ReactiveObject
    {
        private static readonly Random random = new Random();
        private readonly IReactiveList<DinosaurViewModel> dinosaurs;
        private readonly ObservableAsPropertyHelper<int> totalFossilCount;

        public MainViewModel()
        {
            this.dinosaurs = new ReactiveList<DinosaurViewModel>(
                Data
                    .Dinosaurs
                    .All
                    .Take(10)
                    .Select(dinosaur => new DinosaurViewModel(dinosaur.Name, random.Next(0, 10))));
            this.dinosaurs.ChangeTrackingEnabled = true;

            this.totalFossilCount = this
                .dinosaurs
                .ItemChanged
                .Select(_ => this.CountFossils())
                .ToProperty(this, x => x.TotalFossilCount, initialValue: this.CountFossils());
        }

        public IReactiveList<DinosaurViewModel> Dinosaurs => this.dinosaurs;

        public int TotalFossilCount => this.totalFossilCount.Value;

        private int CountFossils() =>
            this
                .dinosaurs
                .Sum(dinosaur => dinosaur.FossilCount.GetValueOrDefault());
    }
}