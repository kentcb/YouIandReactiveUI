namespace Book.ViewModels.Samples.Chapter04.Sample05
{
    using System.Reactive;
    using System.Reactive.Linq;
    using System.Text;
    using global::Splat;
    using ReactiveUI;

    [Sample(
        "Logging",
        @"This sample demonstrates Splat's logging infrastructure.

By registering a custom instance of `Splat.ILogger` in the service locator, this sample is able to collect and display all logging calls made by the application. Try entering details of a fossil discovery and clicking the **LOG** button.")]
    public sealed class MainViewModel : ReactiveObject
    {
        private readonly ReactiveCommand<Unit, Unit> logCommand;
        private readonly ObservableAsPropertyHelper<string> logOutput;
        private string species;
        private string location;

        static MainViewModel()
        {
            // This is how we register our logger implementation. ReactiveUI and anything else using Splat logging will henceforth call into
            // our logger. You would normally do this registration on application startup, not in a view model like this.
            Locator
                .CurrentMutable
                .RegisterConstant(ObservableLogger.Instance, typeof(ILogger));
        }

        public MainViewModel()
        {
            this.logCommand = ReactiveCommand
                .Create(
                    () =>
                    {
                        if (string.IsNullOrEmpty(this.Species))
                        {
                            this
                                .Log()
                                .Error("No species was specified!");
                        }

                        if (string.IsNullOrEmpty(this.Location))
                        {
                            this
                                .Log()
                                .Warn("No location was specified!");
                        }

                        this
                            .Log()
                            .Info("Species {0} was discovered at {1}.", this.Species, this.Location);
                    },
                    outputScheduler: RxApp.MainThreadScheduler);

            this.logOutput = ObservableLogger
                .Instance
                .Messages
                .Scan(new StringBuilder(), (sb, next) => sb.Append("[").Append(next.Item1.ToString()).Append("] ").AppendLine(next.Item2))
                .Select(x => x.ToString())
                .ToProperty(this, x => x.LogOutput, scheduler: RxApp.MainThreadScheduler);

            this
                .Log()
                .Info("Initialized.");
        }

        public ReactiveCommand<Unit, Unit> LogCommand => this.logCommand;

        public string LogOutput => this.logOutput.Value;

        public string Species
        {
            get => this.species;
            set => this.RaiseAndSetIfChanged(ref this.species, value);
        }

        public string Location
        {
            get => this.location;
            set => this.RaiseAndSetIfChanged(ref this.location, value);
        }
    }
}