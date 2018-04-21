namespace Book.ViewModels.Samples.Chapter08.Sample10
{
    using System.Linq;
    using System.Reactive;
    using System.Reactive.Linq;
    using global::Splat;
    using ReactiveUI;

    [Sample(
        "Invoke Command",
        @"This sample demonstrates the use of `InvokeCommand` to invoke a command whenever an observable ticks. You can left double-click the image to zoom in, and right double-click to zoom out.

The `ZoomInCommand` and `ZoomOutCommand` exposed by the view model are executed in response to observable pipelines ticking values in the view. This is achieved by using `InvokeCommand`.")]
    public sealed class MainViewModel : ReactiveObject
    {
        private readonly ReactiveCommand<Unit, Unit> zoomInCommand;
        private readonly ReactiveCommand<Unit, Unit> zoomOutCommand;
        private readonly ObservableAsPropertyHelper<IBitmap> image;
        private float zoom;

        public MainViewModel()
        {
            this.zoom = 1f;
            this.image = Data
                .Dinosaurs
                .All
                .First(x => x.ImageResourceName != null)
                .GetBitmap()
                .ToProperty(this, x => x.Image, scheduler: RxApp.MainThreadScheduler);

            var canZoomIn = this
                .WhenAnyValue(x => x.Zoom)
                .Select(zoom => zoom <= 3);
            this.zoomInCommand = ReactiveCommand.Create(() => { this.Zoom += 0.1f; }, canZoomIn);

            var canZoomOut = this
                .WhenAnyValue(x => x.Zoom)
                .Select(zoom => zoom >= 0.2);
            this.zoomOutCommand = ReactiveCommand.Create(() => { this.Zoom -= 0.1f; }, canZoomOut);
        }

        public ReactiveCommand<Unit, Unit> ZoomInCommand => this.zoomInCommand;

        public ReactiveCommand<Unit, Unit> ZoomOutCommand => this.zoomOutCommand;

        public IBitmap Image => this.image.Value;

        public float Zoom
        {
            get => this.zoom;
            set => this.RaiseAndSetIfChanged(ref this.zoom, value);
        }
    }
}