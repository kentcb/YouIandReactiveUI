namespace Book.ViewModels.Samples.Chapter04.Sample04
{
    using ReactiveUI;

    [Sample(
        "Service Locator",
        @"This sample demonstrates the use of Splat's service locator.

There are two views below: one on the left, and one on the right. They do not have any dependency on each other. Instead, they collaborate via the service locator. The left view allows you to register a new `IDinosaurExhibition` instance in the service locator. The right lists all registered dinosaur exhibitions, refreshing whenever a new one is registered.")]
    public sealed class MainViewModel : ReactiveObject
    {
        private readonly RegisterExhibitionViewModel registerExhibition;
        private readonly ListExhibitionsViewModel listExhibitions;

        public MainViewModel()
        {
            this.registerExhibition = new RegisterExhibitionViewModel();
            this.listExhibitions = new ListExhibitionsViewModel();
        }

        public RegisterExhibitionViewModel RegisterExhibition => this.registerExhibition;

        public ListExhibitionsViewModel ListExhibitions => this.listExhibitions;
    }
}