namespace Book.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive;
    using System.Reactive.Linq;
    using System.Reflection;
    using ReactiveUI;

    public sealed class BookViewModel : ReactiveObject
    {
        private readonly ICollection<SampleWithChapterViewModel> samplesWithChapters;
        private readonly IReactiveDerivedList<SampleWithChapterViewModel> filteredSamplesWithChapters;
        private readonly ReactiveCommand<Unit, Unit> forceGcCommand;
        private readonly ReactiveCommand<Unit, Unit> filterCommand;
        private SampleWithChapterViewModel selectedSampleWithChapter;
        private string filter;

        public BookViewModel()
        {
            var suspensionHost = RxApp.SuspensionHost;
            var appState = suspensionHost.GetAppState<AppState>();
            this.filter = appState.Filter;

            this
                .WhenAnyValue(x => x.Filter)
                .Do(filter => suspensionHost.GetAppState<AppState>().Filter = filter)
                .Subscribe();

            this.samplesWithChapters = this
                .Chapters
                .SelectMany((chapter) => chapter.Samples.Select(sample => new SampleWithChapterViewModel(chapter, sample, this.SelectSample)))
                .ToList();
            this.filterCommand = ReactiveCommand
                .Create(
                    () => { },
                    outputScheduler: RxApp.MainThreadScheduler);

            // filter a short while after the Filter property changes, or immediately if the FilterCommand executes
            var filterSignal = this
                .WhenAnyValue(x => x.Filter)
                .Where(filter => filter != null)
                .Skip(1)
                .Throttle(TimeSpan.FromMilliseconds(250), RxApp.MainThreadScheduler)
                .Select(_ => Unit.Default)
                .Merge(this.filterCommand);
            this.filteredSamplesWithChapters = this
                .samplesWithChapters
                .CreateDerivedCollection(
                    x => x,
                    this.FilterSampleWithChapter,
                    signalReset: filterSignal);
            this.forceGcCommand = ReactiveCommand.Create(
                () =>
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                });
        }

        public string Title => "You, I, and ReactiveUI Samples";

        public ReactiveCommand<Unit, Unit> ForceGCCommand => this.forceGcCommand;

        public ReactiveCommand<Unit, Unit> FilterCommand => this.filterCommand;

        public string Filter
        {
            get => this.filter;
            set => this.RaiseAndSetIfChanged(ref this.filter, value);
        }

        public IEnumerable<ChapterViewModel> Chapters =>
            this
                .GetType()
                .GetTypeInfo()
                .Assembly
                .DefinedTypes
                .Where(x => x.IsSubclassOf(typeof(ChapterViewModel)))
                .Select(x => x.AsType())
                .Select(Activator.CreateInstance)
                .Cast<ChapterViewModel>()
                .OrderBy(chapter => chapter.Number);

        public ICollection<SampleWithChapterViewModel> SamplesWithChapters => this.samplesWithChapters;

        public IReactiveDerivedList<SampleWithChapterViewModel> FilteredSamplesWithChapters => this.filteredSamplesWithChapters;

        public SampleWithChapterViewModel SelectedSampleWithChapter
        {
            get => this.selectedSampleWithChapter;
            set => this.RaiseAndSetIfChanged(ref this.selectedSampleWithChapter, value);
        }

        private bool FilterSampleWithChapter(SampleWithChapterViewModel item)
        {
            if (string.IsNullOrWhiteSpace(this.Filter))
            {
                return true;
            }

            var filter = this.Filter.ToLower();

            // pretty inefficient search, but can be easily improved if necessary
            if (item.Chapter.Name.ToLower().Contains(filter) ||
                item.Sample.Name.ToLower().Contains(filter) ||
                item.Sample.Description.ToLower().Contains(filter) ||
                item.Id.Contains(filter))
            {
                return true;
            }

            return false;
        }

        private void SelectSample((int chapterNumber, int sampleNumber)? sampleInfo)
        {
            if (!sampleInfo.HasValue)
            {
                this.SelectedSampleWithChapter = null;
                return;
            }

            var match = this
                .SamplesWithChapters
                .FirstOrDefault(sampleWithChapter => sampleWithChapter.Chapter.Number == sampleInfo.Value.chapterNumber && sampleWithChapter.Sample.Number == sampleInfo.Value.sampleNumber);

            if (match == null)
            {
                return;
            }

            this.SelectedSampleWithChapter = match;
        }
    }
}