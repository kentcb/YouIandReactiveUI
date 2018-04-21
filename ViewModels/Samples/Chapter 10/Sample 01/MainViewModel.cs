namespace Book.ViewModels.Samples.Chapter10.Sample01
{
    using System;
    using System.Linq;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using ReactiveUI;

    [Sample(
        "Collection change notifications",
        @"This sample demonstrates the use of `ReactiveList<T>` to ensure the UI stays up to date when items are added to, or removed from, a list.

Every second, a dinosaur is randomly added or removed from the list. A current count of dinosaurs is created by hooking into `CountChanged`.")]
    public sealed class MainViewModel : ReactiveObject, ISupportsActivation
    {
        private static readonly Random random = new Random();
        private readonly ViewModelActivator activator;
        private readonly IReactiveList<string> dinosaurs;
        private readonly ObservableAsPropertyHelper<int> count;

        public MainViewModel()
        {
            this.activator = new ViewModelActivator();
            this.dinosaurs = new ReactiveList<string>(
                Data
                    .Dinosaurs
                    .All
                    .Take(10)
                    .Select(dinosaur => dinosaur.Name));
            this.count = this
                .dinosaurs
                .CountChanged
                .ToProperty(this, x => x.Count, initialValue: this.dinosaurs.Count);

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

        public IReactiveList<string> Dinosaurs => this.dinosaurs;

        public int Count => this.count.Value;

        private void MakeRandomChange()
        {
            var add = random.Next(2) == 1;

            if (add || this.dinosaurs.Count == 0)
            {
                var dinosaur = Data
                    .Dinosaurs
                    .All[random.Next(0, Data.Dinosaurs.All.Count)];
                this.dinosaurs.Add(dinosaur.Name);
            }
            else
            {
                var index = random.Next(0, this.dinosaurs.Count);
                this.dinosaurs.RemoveAt(index);
            }
        }
    }
}