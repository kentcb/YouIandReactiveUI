namespace Book.ViewModels.Samples.Chapter04.Sample02
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Reactive.Linq;
    using Data;
    using global::Splat;
    using ReactiveUI;

    [Sample(
        "Geometry",
        @"This sample uses Splat's geometry abstractions to allow users to select points or areas on the map. Choosing a point on the map will cause the view model to locate the nearest fossil, whilst marking out an area will cause it to count the number of fossils within. In both cases, the view model is working with platform-independent geometry types provided by Splat.")]
    public sealed class MainViewModel : ReactiveObject
    {
        private readonly ObservableAsPropertyHelper<IBitmap> worldMap;
        private readonly ObservableAsPropertyHelper<DinosaurFossilLocation?> nearestFossil;
        private readonly ObservableAsPropertyHelper<PointF?> nearestFossilPoint;
        private readonly ObservableAsPropertyHelper<int?> fossilCount;
        private readonly ObservableAsPropertyHelper<string> display;
        private PointF? selectedPoint;
        private RectangleF? selectedArea;

        public MainViewModel()
        {
            var data = Data
                .Dinosaurs
                .All
                .Where(model => model.FossilLocations != null)
                .ToList();
            this.worldMap = Data
                .WorldMap
                .GetBitmap()
                .ToProperty(this, x => x.WorldMap, scheduler: RxApp.MainThreadScheduler);

            this.nearestFossil = this
                .WhenAnyValue(x => x.SelectedPoint)
                .Select(selectedPoint => selectedPoint == null ? (DinosaurFossilLocation?)null : this.FindNearestFossil(data, selectedPoint.Value))
                .ToProperty(this, x => x.NearestFossil);
            this.fossilCount = this
                .WhenAnyValue(x => x.SelectedArea)
                .Select(selectedArea => selectedArea == null ? (int?)null : this.CountFossilsInArea(data, selectedArea.Value))
                .ToProperty(this, x => x.FossilCount);

            // SelectedPoint and SelectedArea are mutually exclusive
            this
                .WhenAnyValue(x => x.SelectedPoint)
                .Where(x => x != null)
                .Do(_ => this.SelectedArea = null)
                .Subscribe();
            this
                .WhenAnyValue(x => x.SelectedArea)
                .Where(x => x != null)
                .Do(_ => this.SelectedPoint = null)
                .Subscribe();

            this.nearestFossilPoint = this
                .WhenAnyValue(x => x.NearestFossil)
                .Select(dinosaurFossilLocation => dinosaurFossilLocation?.FossilLocation)
                .ToProperty(this, x => x.NearestFossilPoint);
            this.display = this
                .WhenAnyValue(x => x.NearestFossil, x => x.FossilCount, (nearestFossil, fossilCount) => (nearestFossil: nearestFossil, fossilCount: fossilCount))
                .Select(
                    info =>
                    {
                        if (info.nearestFossil != null)
                        {
                            return $"The closest fossil to the selected location is a {info.nearestFossil.Value.Dinosaur.Name}.";
                        }
                        else if (info.fossilCount != null)
                        {
                            return $"There are {info.fossilCount} fossils within the selected area.";
                        }

                        return null;
                    })
                .ToProperty(this, x => x.Display);
        }

        public IBitmap WorldMap => this.worldMap.Value;

        public PointF? SelectedPoint
        {
            get => this.selectedPoint;
            set => this.RaiseAndSetIfChanged(ref this.selectedPoint, value);
        }

        public RectangleF? SelectedArea
        {
            get => this.selectedArea;
            set => this.RaiseAndSetIfChanged(ref this.selectedArea, value);
        }

        public PointF? NearestFossilPoint => this.nearestFossilPoint.Value;

        public string Display => this.display.Value;

        private DinosaurFossilLocation? NearestFossil => this.nearestFossil.Value;

        private int? FossilCount => this.fossilCount.Value;

        private DinosaurFossilLocation FindNearestFossil(IEnumerable<Dinosaur> data, PointF selectedPoint) =>
            // use some LINQ to find the nearest fossil to the selected point
            data
                .SelectMany(
                    dinosaur =>
                        dinosaur
                            .FossilLocations
                            ?.Select(fossilLocation => new DinosaurFossilLocation(dinosaur, fossilLocation)) ?? Enumerable.Empty<DinosaurFossilLocation>())
                .Select(dinosaurFossilLocation => (dinosaur: dinosaurFossilLocation.Dinosaur, fossilLocation: dinosaurFossilLocation.FossilLocation, locationDistance: GetLineLength(selectedPoint, dinosaurFossilLocation.FossilLocation)))
                .OrderBy(x => x.locationDistance)
                .Select(info => new DinosaurFossilLocation(info.dinosaur, info.fossilLocation))
                .FirstOrDefault();

        private int CountFossilsInArea(IEnumerable<Dinosaur> data, RectangleF area) =>
            // use some LINQ to find all fossils within a given area
            data
                .SelectMany(
                    dinosaur =>
                        dinosaur
                            .FossilLocations
                            .Select(
                                fossilLocation =>
                                    new DinosaurFossilLocation(
                                        dinosaur,
                                        fossilLocation)))
                .Select(dinosaurFossilLocation => dinosaurFossilLocation.FossilLocation)
                .Where(fossilLocation => area.Contains(fossilLocation))
                .Count();

        private static double GetLineLength(PointF start, PointF end)
        {
            // use Pythagoras to calculate line length
            var width = Math.Abs(end.X - start.X);
            var height = Math.Abs(end.Y - start.Y);
            var length = Math.Sqrt(Math.Pow(width, 2) + Math.Pow(height, 2));
            return length;
        }

        private struct DinosaurFossilLocation
        {
            private readonly Dinosaur dinosaur;
            private readonly PointF fossilLocation;

            public DinosaurFossilLocation(
                Dinosaur dinosaur,
                PointF fossilLocation)
            {
                this.dinosaur = dinosaur;
                this.fossilLocation = fossilLocation;
            }

            public Dinosaur Dinosaur => this.dinosaur;

            public PointF FossilLocation => this.fossilLocation;
        }
    }
}