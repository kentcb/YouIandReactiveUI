namespace Book.ViewModels.Samples.Chapter02.Sample01
{
    using ReactiveUI;

    public sealed class PathViewModel : ReactiveObject
    {
        private readonly string group;
        private readonly string data;
        private readonly string clip;
        private PaletteEntryViewModel paletteEntry;
        private bool highlight;

        public PathViewModel(string group, string data, string clip = null)
        {
            this.group = group;
            this.data = data;
            this.clip = clip;
        }

        public string Group => this.group;

        public string Data => this.data;

        public string Clip => this.clip;

        public PaletteEntryViewModel PaletteEntry
        {
            get => this.paletteEntry;
            set => this.RaiseAndSetIfChanged(ref this.paletteEntry, value);
        }

        public bool Highlight
        {
            get => this.highlight;
            set => this.RaiseAndSetIfChanged(ref this.highlight, value);
        }
    }
}