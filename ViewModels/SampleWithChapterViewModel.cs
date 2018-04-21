namespace Book.ViewModels
{
    using System;
    using System.IO;
    using ReactiveUI;

    public sealed class SampleWithChapterViewModel : ReactiveObject
    {
        private readonly ChapterViewModel chapter;
        private readonly SampleDetailsViewModel sample;
        private readonly Action<(int, int)?> selectSample;

        public SampleWithChapterViewModel(
            ChapterViewModel chapter,
            SampleDetailsViewModel sample,
            Action<(int, int)?> selectSample)
        {
            this.chapter = chapter;
            this.sample = sample;
            this.selectSample = selectSample;
        }

        public ChapterViewModel Chapter => this.chapter;

        public SampleDetailsViewModel Sample => this.sample;

        public string Id => $"{Chapter.NumberDisplay}.{Sample.NumberDisplay}";

        public string GetViewsPathFromSolution(string solutionRoot) =>
            Path.Combine(
                solutionRoot,
                "Host.WPF",
                "Views",
                "Samples",
                $"Chapter {Chapter.NumberDisplay}",
                $"Sample {Sample.NumberDisplay}");

        public string GetViewModelsPathFromSolution(string solutionRoot) =>
            Path.Combine(
                solutionRoot,
                "ViewModels",
                "Samples",
                $"Chapter {Chapter.NumberDisplay}",
                $"Sample {Sample.NumberDisplay}");

        public void SelectSample(string link)
        {
            var parts = link.Split('.');

            if (parts.Length != 2)
            {
                return;
            }

            if (!int.TryParse(parts[0], out var chapterNumber) || !int.TryParse(parts[1], out var sampleNumber))
            {
                return;
            }

            this.SelectSample((chapterNumber, sampleNumber));
        }

        public void SelectSample((int chapterNumber, int sampleNumber)? sampleInfo) =>
            this.selectSample(sampleInfo);
    }
}