namespace Book.ViewModels.Samples.Chapter09.Sample04
{
    using ReactiveUI;

    [Sample(
        "Shared Interactions",
        @"This sample demonstrates the use of a shared interaction.

There are actually two views below, `MainView` and `ChildView`. `MainView` does nothing other than host `ChildView`, and register a default handler against a shared interaction. `ChildView` registers its own handler as well, but it only handles specific interactions.

The `BookReservationCommand` chooses randomly between succeeding, failing with an `IOException`, and failing with a general exception. All exceptions are routed through the shared `Interaction<Exception, Unit>` instance.

The interaction handler in the `ChildView` deals with any `IOException`, whilst the handler in `MainView` deals with all other exceptions.")]
    public sealed class MainViewModel : ReactiveObject
    {
        private readonly ChildViewModel child;

        public MainViewModel()
        {
            this.child = new ChildViewModel();
        }

        public ChildViewModel Child => this.child;
    }
}