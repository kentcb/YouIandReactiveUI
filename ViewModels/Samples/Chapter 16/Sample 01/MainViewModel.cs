namespace Book.ViewModels.Samples.Chapter16.Sample01
{
    using ReactiveUI;

    [Sample(
        "Simple",
        @"This sample demonstrates the use of `ViewModelViewHost` to resolve and display the correct view for a given view model.

The `MainViewModel` exposes a `CountdownViewModel` property, which returns a value of type `CountdownViewModel`. This view model is assigned to the `ViewModelViewHost.ViewModel` property, whereupon the correct view is resolved and displayed.")]
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