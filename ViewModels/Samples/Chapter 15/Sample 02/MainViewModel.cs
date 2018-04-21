namespace Book.ViewModels.Samples.Chapter15.Sample02
{
    using System.Linq;
    using System.Reactive;
    using ReactiveUI;

    [Sample(
        "Contracts",
        @"This sample demonstrates the use of the view locator and contracts to resolve and display the correct view for a given view model in a given context.

The `MainViewModel` exposes a `DinosaurViewModel` property, which returns a value of type `DinosaurViewModel`. It also has a `ViewMode` property, which is used to determine whether the user is displayed an image of the dinosaur, or its details. The selected view mode determines the contract used to resolve the appropriate view.")]
    public sealed class MainViewModel : ReactiveObject
    {
        private readonly DinosaurViewModel dinosaurViewModel;
        private readonly ReactiveCommand<Unit, ViewMode> toggleViewModeCommand;
        private ViewMode viewMode;

        public MainViewModel()
        {
            var data = Data
                .Dinosaurs
                .All
                .Where(dinosaur => dinosaur.ImageResourceName != null)
                .Skip(3)    // Just to get some different images in the book screenshots
                .First();
            this.dinosaurViewModel = new DinosaurViewModel(data);

            this.toggleViewModeCommand = ReactiveCommand.Create(
                () => this.ViewMode = this.ViewMode.Toggle());
        }

        public DinosaurViewModel DinosaurViewModel => this.dinosaurViewModel;

        public ReactiveCommand<Unit, ViewMode> ToggleViewModeCommand => this.toggleViewModeCommand;

        public ViewMode ViewMode
        {
            get => this.viewMode;
            set => this.RaiseAndSetIfChanged(ref this.viewMode, value);
        }
    }
}