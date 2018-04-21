namespace Book.ViewModels.Samples.Chapter17.Sample02
{
    using System.Linq;
    using System.Reactive;
    using System.Reactive.Linq;
    using ReactiveUI;

    [Sample(
        "Complex",
        @"This is a more complex and realistic example of using ReactiveUI's routing infrastructure.

In it, the `MainViewModel` implements `IScreen` and exposes navigation functionality relevant to all views, such as a breadcrumb and navigation commands.

The user is able to navigate through a non-trivial tree of views by selecting items they're interested in.")]
    public sealed class MainViewModel : ReactiveObject, IScreen
    {
        private readonly RoutingState routingState;
        private readonly ReactiveCommand<Unit, Unit> startAgainCommand;
        private readonly ObservableAsPropertyHelper<string> title;
        private readonly ObservableAsPropertyHelper<string> breadcrumb;

        public MainViewModel()
        {
            this.routingState = new RoutingState();

            this.startAgainCommand = ReactiveCommand.CreateFromObservable(
                () => this.routingState.NavigateAndReset.Execute(new TopicsViewModel(this)).Select(_ => Unit.Default));

            this.title = this
                .routingState
                .CurrentViewModel
                .Select(x => x?.UrlPathSegment ?? null)
                .ToProperty(this, x => x.Title);

            this.breadcrumb = this
                .routingState
                .CurrentViewModel
                .Select(_ => DetermineBreadcrumb(this.routingState.NavigationStack))
                .ToProperty(this, x => x.Breadcrumb);

            this.routingState.Navigate.Execute(new TopicsViewModel(this));
        }

        public RoutingState Router => this.routingState;

        public ReactiveCommand<Unit, Unit> NavigateBackCommand => this.routingState.NavigateBack;

        public ReactiveCommand<Unit, Unit> StartAgainCommand => this.startAgainCommand;

        public string Title => this.title.Value;

        public string Breadcrumb => this.breadcrumb.Value;

        private static string DetermineBreadcrumb(ReactiveList<IRoutableViewModel> stack) =>
            stack
                .Take(stack.Count - 1)
                .Aggregate(
                    "",
                    (acc, next) => acc + next.UrlPathSegment + " > ");
    }
}