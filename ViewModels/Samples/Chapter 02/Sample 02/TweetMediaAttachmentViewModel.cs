namespace Book.ViewModels.Samples.Chapter02.Sample02
{
    using System.IO;
    using System.Reactive.Linq;
    using System.Reactive.Threading.Tasks;
    using ReactiveUI;
    using Splat;

    public sealed class TweetMediaAttachmentViewModel : ReactiveObject
    {
        private readonly byte[] imageData;
        private readonly ObservableAsPropertyHelper<IBitmap> bitmap;

        public TweetMediaAttachmentViewModel(byte[] imageData)
        {
            this.imageData = imageData;

            this.bitmap = Observable
                .Using(
                    () => new MemoryStream(imageData),
                    stream =>
                        BitmapLoader
                            .Current
                            .Load(stream, desiredWidth: null, desiredHeight: null)
                            .ToObservable())
                .ToProperty(this, x => x.Bitmap, scheduler: RxApp.MainThreadScheduler);
        }

        public byte[] ImageData => this.imageData;

        public IBitmap Bitmap => this.bitmap.Value;
    }
}