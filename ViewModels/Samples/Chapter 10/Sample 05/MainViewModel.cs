namespace Book.ViewModels.Samples.Chapter10.Sample05
{
    using System;
    using System.Linq;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using ReactiveUI;

    [Sample(
        "CreateDerivedList: sort",
        @"This sample demonstrates the use of `CreateDerivedList` to create a sorted list based on the contents of another.

The view model contains a `ReactiveList<Dinosaur>`, which is randomly modified every second. It derives a second list of type `IReactiveDerivedList<DinosaurViewModel>` from the list of models, using the `selector` parameter to convert each model to a view model, and the `orderer` to ensure dinosaurs are sorted alphabetically.")]
    public sealed class MainViewModel : ReactiveObject, ISupportsActivation
    {
        private static readonly Random random = new Random();
        private readonly ViewModelActivator activator;
        private readonly IReactiveList<Data.Dinosaur> dinosaurModels;
        private readonly IReactiveDerivedList<DinosaurViewModel> dinosaurs;

        public MainViewModel()
        {
            this.activator = new ViewModelActivator();
            this.dinosaurModels = new ReactiveList<Data.Dinosaur>(
                Data
                    .Dinosaurs
                    .All
                    .Take(10));
            this.dinosaurs = this
                .dinosaurModels
                .CreateDerivedCollection(
                    selector: dinosaur => new DinosaurViewModel(dinosaur.Name),
                    orderer: (first, second) => string.Compare(first.Name, second.Name));

            this
                .WhenActivated(
                    disposables =>
                    {
                        Observable
                            .Timer(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1), RxApp.MainThreadScheduler)
                            .Do(_ => MakeRandomChange())
                            .Subscribe()
                            .DisposeWith(disposables);
                    });
        }

        public ViewModelActivator Activator => this.activator;

        public IReactiveDerivedList<DinosaurViewModel> Dinosaurs => this.dinosaurs;

        private void MakeRandomChange()
        {
            var add = random.Next(2) == 1;

            if (add || this.dinosaurs.Count == 0)
            {
                var dinosaur = Data
                    .Dinosaurs
                    .All[random.Next(0, Data.Dinosaurs.All.Count)];
                this.dinosaurModels.Add(dinosaur);
            }
            else
            {
                var index = random.Next(0, this.dinosaurs.Count);
                this.dinosaurModels.RemoveAt(index);
            }
        }
    }
}