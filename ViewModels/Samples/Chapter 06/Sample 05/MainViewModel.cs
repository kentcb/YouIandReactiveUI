namespace Book.ViewModels.Samples.Chapter06.Sample05
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using ReactiveUI;

    [Sample(
        "WhenAnyObservable",
        @"This sample demonstrates the use of `WhenAnyObservable` to monitor an observable exposed via a property.

Each of the exhibitions in the list is backed by an `ExhibitionViewModel`. It exposes a property called `OpenCountdown` of type `IObservable<TimeSpan>`. The `MainViewModel` uses `WhenAnyObservable` to dereference the `OpenCountdown` for the selected exhibition. From there it creates the display string shown on the right.")]
    public sealed class MainViewModel : ReactiveObject, ISupportsActivation
    {
        private readonly ViewModelActivator activator;
        private readonly List<ExhibitionViewModel> exhibitions;
        private ExhibitionViewModel selectedExhibition;
        private string countdown;

        public MainViewModel()
        {
            this.activator = new ViewModelActivator();
            this.exhibitions = new[]
            {
                new ExhibitionViewModel("Fossils Alive!", DateTime.Now.AddSeconds(30)),
                new ExhibitionViewModel("Jurassic Jungle", DateTime.Now.AddMinutes(1)),
                new ExhibitionViewModel("Walk with the Dinosaurs", DateTime.Now.AddMinutes(3))
            }.ToList();

            this
                .WhenActivated(
                    disposables =>
                    {
                        this
                            .WhenAnyObservable(x => x.SelectedExhibition.OpenCountdown)
                            .Select(
                                timeLeft =>
                                {
                                    if (timeLeft < TimeSpan.Zero)
                                    {
                                        return "This exhibition is open!";
                                    }
                                    else if (timeLeft < TimeSpan.FromSeconds(10))
                                    {
                                        return "This exhibition is opening very soon...";
                                    }
                                    else
                                    {
                                        return $"This exhibition opens in {timeLeft.TotalSeconds:N0} seconds.";
                                    }
                                })
                            .Do(countdown => this.Countdown = countdown)
                            .Subscribe()
                            .DisposeWith(disposables);
                    });
        }

        public ViewModelActivator Activator => this.activator;

        public IList<ExhibitionViewModel> Exhibitions => this.exhibitions;

        public ExhibitionViewModel SelectedExhibition
        {
            get => this.selectedExhibition;
            private set => this.RaiseAndSetIfChanged(ref this.selectedExhibition, value);
        }

        public string Countdown
        {
            get => this.countdown;
            private set => this.RaiseAndSetIfChanged(ref this.countdown, value);
        }
    }
}