namespace Book.ViewModels.Samples.Chapter04.Sample03
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;
    using Data;
    using ReactiveUI;

    [Sample(
        "Memoizing MRU Cache",
        @"This sample demonstrates the use of Splat's `MemoizingMRUCache<TParam, TVal>` type.

Brushes for the color swatches are cached in a `MemoizingMRUCache<Color, Brush>` with a capacity of just 2 items. The list is virtualized, so as you scroll through it extra list items are being realized and rendered. The options allow you to control various aspects of the behavior, and there are useful statistics displayed so you can get a sense for how the caching is affecting the performance of the application.")]
    public sealed class MainViewModel : ReactiveObject, ISupportsActivation
    {
        private readonly ViewModelActivator activator;
        private readonly ObservableAsPropertyHelper<IList<DinosaurViewModel>> dinosaurs;
        private readonly IList<ColorKeyViewModel> colorKeys;
        private readonly Subject<Unit> reset;
        private bool isCachingEnabled;
        private bool isDataOrderCacheFriendly;
        private bool isFakeDelayImposed;
        private int resourcesRequested;
        private int resourcesCreated;
        private int resourcesReleased;

        public MainViewModel()
        {
            this.activator = new ViewModelActivator();
            this.isCachingEnabled = true;
            this.isDataOrderCacheFriendly = true;
            this.reset = new Subject<Unit>();

            this.dinosaurs = this
                .WhenAnyValue(x => x.IsDataOrderCacheFriendly, x => x.IsCachingEnabled, (isDataOrderCacheFriendly, _) => isDataOrderCacheFriendly)
                .Select(GetDinosaurViewModels)
                .Do(_ => this.ResetCounters())
                .ToProperty(this, x => x.Dinosaurs);

            this.colorKeys = TimelineColors
                .GetColors()
                .Select(colorInfo => new ColorKeyViewModel(colorInfo.era, colorInfo.period, colorInfo.color))
                .ToList();

            this
                .WhenActivated(
                    disposables =>
                    {
                        Disposable
                            .Create(() => this.ResetCounters())
                            .DisposeWith(disposables);
                    });
        }

        public ViewModelActivator Activator => this.activator;

        public IList<DinosaurViewModel> Dinosaurs => this.dinosaurs.Value;

        public IList<ColorKeyViewModel> ColorKeys => this.colorKeys;

        public bool IsCachingEnabled
        {
            get => this.isCachingEnabled;
            set => this.RaiseAndSetIfChanged(ref this.isCachingEnabled, value);
        }

        public bool IsDataOrderCacheFriendly
        {
            get => this.isDataOrderCacheFriendly;
            set => this.RaiseAndSetIfChanged(ref this.isDataOrderCacheFriendly, value);
        }

        public bool IsFakeDelayImposed
        {
            get => this.isFakeDelayImposed;
            set => this.RaiseAndSetIfChanged(ref this.isFakeDelayImposed, value);
        }

        public int ResourcesRequested
        {
            get => this.resourcesRequested;
            set => this.RaiseAndSetIfChanged(ref this.resourcesRequested, value);
        }

        public int ResourcesCreated
        {
            get => this.resourcesCreated;
            set => this.RaiseAndSetIfChanged(ref this.resourcesCreated, value);
        }

        public int ResourcesReleased
        {
            get => this.resourcesReleased;
            set => this.RaiseAndSetIfChanged(ref this.resourcesReleased, value);
        }

        public IObservable<Unit> Reset => this.reset;

        private void ResetCounters()
        {
            this.reset.OnNext(Unit.Default);

            this.ResourcesRequested = 0;
            this.ResourcesCreated = 0;
            this.ResourcesReleased = 0;
        }

        private static IList<DinosaurViewModel> GetDinosaurViewModels(bool isDataOrderCacheFriendly)
        {
            var viewModels = Data
                .Dinosaurs
                .All
                .Where(model => model.Era.HasValue && model.Period.HasValue)
                .Select(model => new DinosaurViewModel(model));

            if (isDataOrderCacheFriendly)
            {
                return viewModels
                    .OrderBy(x => x.Era)
                    .ThenBy(x => x.Period)
                    .ToList();
            }
            else
            {
                var random = new Random();
                return viewModels
                    .OrderBy(_ => random.Next())
                    .ToList();
            }
        }
    }
}