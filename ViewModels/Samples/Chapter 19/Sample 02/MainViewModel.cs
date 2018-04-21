namespace Book.ViewModels.Samples.Chapter19.Sample02
{
    using System.Reactive;
    using System.Reactive.Concurrency;
    using System.Reactive.Linq;
    using ReactiveUI;

    [Sample(
        "Auto persistence: AutoPersist",
        @"This sample uses the `AutoPersist` extension method to automatically propagate changes in a referenced view model to the application state.

The `PersistedViewModel` class is a view model that is persisted by storing a reference to it inside the `AppState` class. By using `AutoPersist`, we can easily ensure any changes make their way into the `AppState` instance.

Note that the `PersistedViewModel.Transient` property is not annotated with `[DataMember]`. As such, it is not saved and restored, and any changes made to it do not trigger a save.")]
    public sealed class MainViewModel : ReactiveObject
    {
        private readonly PersistedViewModel persisted;
        private int saveCount;

        public MainViewModel()
        {
            var suspensionHost = RxApp.SuspensionHost;
            var appState = suspensionHost.GetAppState<AppState>();
            this.persisted = appState.PersistedViewModel ?? new PersistedViewModel();

            this
                .persisted
                .AutoPersist(
                    vm =>
                    {
                        RxApp.MainThreadScheduler.Schedule(() => this.SaveCount += 1);

                        appState.PersistedViewModel = vm;
                        return Observable.Return(Unit.Default);
                    });
        }

        public PersistedViewModel Persisted => this.persisted;

        public int SaveCount
        {
            get => this.saveCount;
            private set => this.RaiseAndSetIfChanged(ref this.saveCount, value);
        }
    }
}