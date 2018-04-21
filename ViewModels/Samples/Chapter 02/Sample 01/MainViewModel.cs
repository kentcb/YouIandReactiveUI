namespace Book.ViewModels.Samples.Chapter02.Sample01
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Drawing;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reactive;
    using System.Reactive.Linq;
    using ReactiveUI;

    [Sample(
        "Paint",
        @"This sample allows you to paint dinosaur pictures using a constrained palette.")]
    public sealed class MainViewModel : ReactiveObject
    {
        private readonly ReactiveCommand<Unit, Unit> clearCommand;
        private readonly Interaction<Unit, bool> confirmClearInteraction;
        private readonly ImmutableArray<IllustrationViewModel> illustrations;
        private readonly ImmutableArray<PaletteEntryViewModel> paletteEntries;
        private IllustrationViewModel selectedIllustration;
        private PaletteEntryViewModel selectedPaletteEntry;
        private bool showOutline;
        private bool paintGroups;

        public MainViewModel()
        {
            this.clearCommand = ReactiveCommand.CreateFromObservable(
                this.Clear);
            this.confirmClearInteraction = new Interaction<Unit, bool>();

            this.illustrations = GetIllustrations()
                .ToImmutableArray();
            this.paletteEntries = GetDefaultPaletteColors()
                .Select((color, index) => new PaletteEntryViewModel(index, color))
                .ToImmutableArray();
            this.showOutline = true;
            this.paintGroups = true;

            this.selectedIllustration = this.illustrations.First();
            this.selectedPaletteEntry = this.paletteEntries.First();

            this
                .WhenGroupingEnabledAndPathChanges(x => x.PaletteEntry)
                .Do(
                    path =>
                    {
                        foreach (var pathInGroup in this.SelectedIllustration.GetPathsInGroup(path.Group))
                        {
                            pathInGroup.PaletteEntry = path.PaletteEntry;
                        }
                    })
                .Subscribe();

            var highlightLatch = false;

            this
                .WhenGroupingEnabledAndPathChanges(x => x.Highlight)
                .Where(_ => !highlightLatch)
                .Do(
                    path =>
                    {
                        highlightLatch = true;

                        try
                        {
                            foreach (var pathInGroup in this.SelectedIllustration.GetPathsInGroup(path.Group))
                            {
                                pathInGroup.Highlight = path.Highlight;
                            }
                        }
                        finally
                        {
                            highlightLatch = false;
                        }
                    })
                .Subscribe();
        }

        public ReactiveCommand<Unit, Unit> ClearCommand => this.clearCommand;

        public Interaction<Unit, bool> ConfirmClearInteraction => this.confirmClearInteraction;

        public ImmutableArray<IllustrationViewModel> Illustrations => this.illustrations;

        public ImmutableArray<PaletteEntryViewModel> PaletteEntries => this.paletteEntries;

        public IllustrationViewModel SelectedIllustration
        {
            get => this.selectedIllustration;
            set => this.RaiseAndSetIfChanged(ref this.selectedIllustration, value);
        }

        public PaletteEntryViewModel SelectedPaletteEntry
        {
            get => this.selectedPaletteEntry;
            set => this.RaiseAndSetIfChanged(ref this.selectedPaletteEntry, value);
        }

        public bool ShowOutline
        {
            get => this.showOutline;
            set => this.RaiseAndSetIfChanged(ref this.showOutline, value);
        }

        public bool PaintGroups
        {
            get => this.paintGroups;
            set => this.RaiseAndSetIfChanged(ref this.paintGroups, value);
        }

        private IObservable<Unit> Clear() =>
            this
                .confirmClearInteraction
                .Handle(Unit.Default)
                .Where(result => result)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Do(
                    _ =>
                    {
                        foreach (var path in this.SelectedIllustration.Paths)
                        {
                            path.PaletteEntry = null;
                        }
                    })
                .Select(_ => Unit.Default);

        private IObservable<PathViewModel> WhenGroupingEnabledAndPathChanges<TDontCare>(Expression<Func<PathViewModel, TDontCare>> selector) =>
            this
                .WhenAnyValue(x => x.PaintGroups, x => x.SelectedIllustration, (paintGroups, _) => paintGroups)
                .Select(
                    paintGroups =>
                        paintGroups ?
                            Observable
                                .Merge(
                                    this
                                        .SelectedIllustration
                                        .Paths
                                        .Select(path => path.WhenAnyValue(selector).Skip(1).Select(_ => path))) :
                            Observable.Never<PathViewModel>())
                .Switch();

        private static IEnumerable<IllustrationViewModel> GetIllustrations()
        {
            yield return Archaeopteryx.CreateIllustration();
            yield return Carnotaurus.CreateIllustration();
            yield return Diplodocus.CreateIllustration();
            yield return Spinosaurus.CreateIllustration();
        }

        private static IEnumerable<Color> GetDefaultPaletteColors()
        {
            yield return Color.Black;
            yield return Color.White;
            yield return Color.FromArgb(96, 96, 96);
            yield return Color.FromArgb(112, 165, 112);
            yield return Color.FromArgb(89, 143, 165);
            yield return Color.FromArgb(163, 153, 44);
            yield return Color.FromArgb(168, 40, 18);
            yield return Color.FromArgb(144, 129, 168);
        }
    }
}