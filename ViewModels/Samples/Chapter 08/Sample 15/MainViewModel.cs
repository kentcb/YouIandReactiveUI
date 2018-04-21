namespace Book.ViewModels.Samples.Chapter08.Sample15
{
    using System;
    using System.Linq;
    using System.Reactive;
    using System.Reactive.Linq;
    using global::Splat;
    using ReactiveUI;

    [Sample(
        "Progress",
        @"This sample demonstrates how a reactive command can tick multiple values during execution, with each containing information about progress.

The `CloneCommand` ticks multiple values (of type `ProgressInfo`) throughout the course of its execution. The view uses the information contained within each `ProgressInfo` to display progress information to you while the command executes.")]
    public sealed class MainViewModel : ReactiveObject
    {
        private readonly ReactiveCommand<Unit, ProgressInfo> cloneCommand;
        private readonly ObservableAsPropertyHelper<IBitmap> image;

        public MainViewModel()
        {
            var ticks = Observable
                .Concat(
                    Observable.Timer(TimeSpan.Zero, TimeSpan.FromMilliseconds(25)).Take(100),
                    Observable.Timer(TimeSpan.FromMilliseconds(750), TimeSpan.FromMilliseconds(50)).Take(100),
                    Observable.Timer(TimeSpan.FromMilliseconds(600), TimeSpan.FromMilliseconds(20)).Take(100));
            var progress = ticks
                .Scan(
                    new ProgressInfo(0, 3, 0, 0),
                    (acc, _) => acc.IncreaseProgress());

            this.cloneCommand = ReactiveCommand.CreateFromObservable(
                () => progress);

            this.image = Data
                .Dinosaurs
                .All
                .Where(x => x.ImageResourceName != null)
                .ElementAt(1)
                .GetBitmap()
                .ToProperty(this, x => x.Image, scheduler: RxApp.MainThreadScheduler);
        }

        public ReactiveCommand<Unit, ProgressInfo> CloneCommand => this.cloneCommand;

        public IBitmap Image => this.image.Value;
    }
}