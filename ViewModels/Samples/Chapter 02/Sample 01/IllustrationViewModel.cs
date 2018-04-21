namespace Book.ViewModels.Samples.Chapter02.Sample01
{
    using Genesis.Ensure;
    using Splat;
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Reactive.Linq;
    using System.Reactive.Threading.Tasks;
    using System.Reflection;

    public sealed class IllustrationViewModel
    {
        private readonly string previewResourceName;
        private readonly double width;
        private readonly double height;
        private readonly ImmutableArray<PathViewModel> paths;

        public IllustrationViewModel(
            string previewResourceName,
            double width,
            double height,
            ImmutableArray<PathViewModel> paths)
        {
            this.previewResourceName = previewResourceName;
            this.width = width;
            this.height = height;
            this.paths = paths;
        }

        public string PreviewResourceName => this.previewResourceName;

        public double Width => this.width;

        public double Height => this.height;

        public ImmutableArray<PathViewModel> Paths => this.paths;

        public IEnumerable<PathViewModel> GetPathsInGroup(string group)
        {
            foreach (var path in this.paths)
            {
                if (path.Group == group)
                {
                    yield return path;
                }
            }
        }

        public IObservable<IBitmap> GetBitmap(bool thumbnail = false) =>
            Observable
                .Using(
                    () => this.GetType().GetTypeInfo().Assembly.GetManifestResourceStream("Book.ViewModels.Data.Images." + this.previewResourceName + (thumbnail ? "_thumb" : "") + ".png"),
                    stream =>
                    {
                        Ensure.ArgumentNotNull(stream, nameof(stream));

                        return BitmapLoader
                            .Current
                            .Load(stream, desiredWidth: null, desiredHeight: null)
                            .ToObservable();
                    });
    }
}