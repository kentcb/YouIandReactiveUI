namespace Book.ViewModels.Samples.Chapter17.Sample01
{
    using ReactiveUI;

    [Sample(
        "Simple",
        @"This sample demonstrates the use of ReactiveUI's routing infrastructure to create a few simple, navigable views.

The `MainViewModel` implements `IScreen`, so it is responsible for holding the navigation stack. It navigates to `BlurbViewModel` by default, which is one of three implementations of `IRoutableViewModel`.")]
    public sealed class MainViewModel : ReactiveObject, IScreen
    {
        private readonly RoutingState routingState;

        public MainViewModel()
        {
            this.routingState = new RoutingState();

            this.routingState.Navigate.Execute(new BlurbViewModel(this));
        }

        public RoutingState Router => this.routingState;
    }
}