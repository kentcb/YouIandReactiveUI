namespace Book.ViewModels.Samples.Chapter19.Sample01
{
    using System;
    using System.Reactive.Linq;
    using ReactiveUI;

    [Sample(
        "Auto persistence: simple",
        @"This sample leverages the suspension host to persist data across application restarts.

The dinosaur name you enter below is copied into the `AppState` object. Consequently, it is saved and will be restored when the application next starts.")]
    public sealed class MainViewModel : ReactiveObject
    {
        private string name;

        public MainViewModel()
        {
            var suspensionHost = RxApp.SuspensionHost;
            var appState = suspensionHost.GetAppState<AppState>();
            this.name = appState.DinosaurName;

            this
                .WhenAnyValue(x => x.Name)
                .Do(name => suspensionHost.GetAppState<AppState>().DinosaurName = name)
                .Subscribe();
        }

        public string Name
        {
            get => this.name;
            set => this.RaiseAndSetIfChanged(ref this.name, value);
        }
    }
}