namespace Book.ViewModels.Samples.Chapter04.Sample01
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive.Linq;
    using ReactiveUI;

    [Sample(
        "Bitmaps",
        @"This sample demonstrates the use of Splat's platform-independent `IBitmap` abstraction. It presents an image of a dinosaur, and allows you to view other dinosaurs.

Each dinosaur is represented by an instance of `DinosaurViewModel`. The `DinosaurViewModel` exposes an `Image` property, which is of type `IBitmap`. The view code binds to this property, converting the platform-independent abstraction into a platform-specific implementation via Splat's `ToNative` extension method.")]
    public sealed class MainViewModel : ReactiveObject
    {
        private readonly IList<DinosaurViewModel> dinosaurs;

        public MainViewModel()
        {
            this.dinosaurs = Data
                .Dinosaurs
                .All
                .Where(model => model.ImageResourceName != null)
                .Select(model => new DinosaurViewModel(model))
                .ToList();
        }

        public IList<DinosaurViewModel> Dinosaurs => this.dinosaurs;
    }
}