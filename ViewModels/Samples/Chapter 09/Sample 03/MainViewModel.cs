namespace Book.ViewModels.Samples.Chapter09.Sample03
{
    using System.Reactive;
    using ReactiveUI;

    [Sample(
        "Interaction with multiple handlers",
        @"This sample demonstrates the precedence mechanism implemented by `Interaction<TInput, TOutput>`.

The view registers always has at least one interaction handler registered. However, when you check the *Don glasses* check box, it registers a second. Due to the precedence mechanism implemented by interactions, the second handler will always be queried before the first, giving it the first opportunity to handle the interaction.")]
    public sealed class MainViewModel : ReactiveObject
    {
        private readonly Interaction<Unit, string> spotDinosaurInteraction;
        private readonly ReactiveCommand<Unit, string> spotDinosaurCommand;
        private readonly ObservableAsPropertyHelper<string> spottedDinosaur;
        private bool donGlasses;

        public MainViewModel()
        {
            this.spotDinosaurInteraction = new Interaction<Unit, string>();
            this.spotDinosaurCommand = ReactiveCommand.CreateFromObservable(
                () =>
                    this
                        .spotDinosaurInteraction
                        .Handle(Unit.Default));
            this.spottedDinosaur = this
                .spotDinosaurCommand
                .ToProperty(this, x => x.SpottedDinosaur);
        }

        public Interaction<Unit, string> SpotDinosaurInteraction => this.spotDinosaurInteraction;

        public ReactiveCommand<Unit, string> SpotDinosaurCommand => this.spotDinosaurCommand;

        public string SpottedDinosaur => this.spottedDinosaur.Value;

        public bool DonGlasses
        {
            get => this.donGlasses;
            set => this.RaiseAndSetIfChanged(ref this.donGlasses, value);
        }
    }
}