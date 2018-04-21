namespace Book.ViewModels.Samples.Chapter07.Sample04
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive.Linq;
    using ReactiveUI;

    [Sample(
        "Lazy ToProperty",
        @"This sample demonstrates the use of lazy `ToProperty` calls to ensure subscription is deferred.

Each dinosaur in the list is represented by an instance of `DinosaurViewModel`. It exposes a `Color` property to determine the color of the swatch beside it. The `Color` property is using `ObservableAsPropertyHelper<T>` with a deferred subscription.

Because the list is virtualized, not all cells are created up-front, and therefore not all `Color` properties are subscribed to. As you scroll down, you will see the subscription count increase. It never decreases because we aren't deactivating view models when they go off screen.")]
    public sealed class MainViewModel : ReactiveObject
    {
        private readonly IList<DinosaurViewModel> dinosaurs;
        private int subscriptionCount;

        public MainViewModel()
        {
            this.dinosaurs = Data
                .Dinosaurs
                .All
                .OrderBy(model => model.Name)
                .Where(model => model.Era.HasValue && model.Period.HasValue)
                .Select(model => new DinosaurViewModel(this, model))
                .ToList();
        }

        public IList<DinosaurViewModel> Dinosaurs => this.dinosaurs;

        public int SubscriptionCount
        {
            get => this.subscriptionCount;
            internal set => this.RaiseAndSetIfChanged(ref this.subscriptionCount, value);
        }
    }
}