namespace Book.ViewModels.Samples.Chapter15.Sample01
{
    using ReactiveUI;

    [Sample(
        "Simple",
        @"This sample demonstrates the use of the view locator to resolve and display the correct view for a given view model.

The `MainViewModel` exposes a `CountdownViewModel` property, which returns a value of type `CountdownViewModel`. This view model is used by the view to resolve the correct child view for display purposes.")]
    public sealed class MainViewModel : ReactiveObject
    {
        private readonly CountdownViewModel countdownViewModel;

        public MainViewModel()
        {
            this.countdownViewModel = new CountdownViewModel();
        }

        public CountdownViewModel CountdownViewModel => this.countdownViewModel;
    }
}