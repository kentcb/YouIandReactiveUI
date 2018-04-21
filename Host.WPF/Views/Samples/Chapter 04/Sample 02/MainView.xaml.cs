namespace Book.Views.Samples.Chapter04.Sample02
{
    using System;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using Behaviors;
    using global::Splat;
    using ReactiveUI;
    using ViewModels.Samples.Chapter04.Sample02;

    public partial class MainView : ReactiveUserControl<MainViewModel>
    {
        public MainView()
        {
            InitializeComponent();

            this
                .WhenActivated(
                    disposables =>
                    {
                        this
                            .OneWayBind(this.ViewModel, x => x.WorldMap, x => x.worldMapImage.Source, x => x?.ToNative())
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.SelectedPoint, x => x.selectedPointIndicator.Visibility, x => x == null ? Visibility.Collapsed : Visibility.Visible)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.SelectedArea, x => x.selectedAreaIndicator.Visibility, x => x == null ? Visibility.Collapsed : Visibility.Visible)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.NearestFossilPoint, x => x.nearestFossilPath.Visibility, x => x == null ? Visibility.Collapsed : Visibility.Visible)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.Display, x => x.nearestFossilDisplayLabel.Content, x => x ?? "Click anywhere on the map to find the nearest fossil, or drag out an area to count the number of fossils within.")
                            .DisposeWith(disposables);
                        this
                            .WhenAnyValue(x => x.ViewModel.SelectedPoint)
                            .Do(selectedPoint => Position.SetCenter(this.selectedPointIndicator, selectedPoint?.ToNative()))
                            .Subscribe()
                            .DisposeWith(disposables);
                        this
                            .WhenAnyValue(x => x.ViewModel.SelectedArea)
                            .Do(
                                selectedArea =>
                                {
                                    var rect = selectedArea?.ToNative();

                                    Position.SetTopLeft(this.selectedAreaIndicator, rect?.TopLeft);
                                    this.selectedAreaIndicator.Width = rect?.Width ?? 0;
                                    this.selectedAreaIndicator.Height = rect?.Height ?? 0;
                                })
                            .Subscribe()
                            .DisposeWith(disposables);
                        this
                            .WhenAnyValue(x => x.ViewModel.NearestFossilPoint)
                            .Do(nearestFossilPoint => Position.SetCenter(this.nearestFossilPath, nearestFossilPoint?.ToNative()))
                            .Subscribe()
                            .DisposeWith(disposables);

                        var mouseLeftButtonDown = this
                            .worldMapImage
                            .Events()
                            .MouseLeftButtonDown
                            .Publish();
                        var mouseRightButtonDown = this
                            .worldMapImage
                            .Events()
                            .MouseRightButtonDown
                            .Publish();
                        var mouseMove = this
                            .worldMapImage
                            .Events()
                            .MouseMove;
                        var mouseLeftButtonUp = this
                            .worldMapImage
                            .Events()
                            .MouseLeftButtonUp
                            .Publish();

                    IObservable<(Point, Point)> MouseMovesWith(Point startingPosition) =>
                        mouseMove
                            .Select(e => (startingPosition, e.GetPosition(this.worldMapImage)));

                    Rect CreateRectFrom((Point startingPosition, Point currentPosition) info) =>
                        new Rect(info.startingPosition, info.currentPosition);

                    mouseLeftButtonDown
                        .Select(e => e.GetPosition(this.worldMapImage))
                        //.Do(_ => this.worldMapImage.CaptureMouse())
                        .SelectMany(MouseMovesWith)
                        .Select(info => CreateRectFrom(info).FromNative())
                        .Do(selectedArea => this.ViewModel.SelectedArea = selectedArea)
                        .TakeUntil(mouseLeftButtonUp)
                        //.Do(_ => this.worldMapImage.ReleaseMouseCapture())
                        .Repeat()
                        .Subscribe()
                        .DisposeWith(disposables);

                        mouseLeftButtonDown
                            .Select(e => e.GetPosition(this.worldMapImage).FromNative())
                            .Do(selectedPoint => this.ViewModel.SelectedPoint = selectedPoint)
                            .Subscribe()
                            .DisposeWith(disposables);
                        mouseRightButtonDown
                            .Do(
                                e =>
                                {
                                    this.ViewModel.SelectedPoint = null;
                                    this.ViewModel.SelectedArea = null;
                                })
                            .Subscribe()
                            .DisposeWith(disposables);

                        mouseLeftButtonDown
                            .Connect()
                            .DisposeWith(disposables);
                        mouseRightButtonDown
                            .Connect()
                            .DisposeWith(disposables);
                        mouseLeftButtonUp
                            .Connect()
                            .DisposeWith(disposables);
                    });
        }
    }
}