namespace Book.ViewModels.Samples.Chapter25.Sample01
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reactive;
    using System.Reactive.Concurrency;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Reactive.Threading.Tasks;
    using ReactiveUI;
    using Splat;

    public sealed class DinosaurDetailsViewModel : ReactiveObject, ISupportsActivation
    {
        private readonly int id;
        private readonly IApi api;
        private readonly ViewModelActivator activator;
        private readonly ReactiveCommand<Unit, Dinosaur> getCommand;
        private readonly ReactiveCommand<Unit, Unit> saveCommand;
        private readonly ReactiveCommand<Unit, Unit> deleteCommand;
        private readonly Interaction<Unit, bool> confirmDeleteInteraction;
        private readonly ObservableAsPropertyHelper<bool> isBusy;
        private readonly ObservableAsPropertyHelper<Error> error;
        private readonly ObservableAsPropertyHelper<Validated<int>> validatedWeight;
        private readonly ObservableAsPropertyHelper<IBitmap> image;
        private string name;
        private string weight;
        private byte[] imageData;

        public DinosaurDetailsViewModel(
            int id,
            IBitmapLoader bitmapLoader,
            IApi api,
            IScheduler scheduler)
        {
            this.id = id;
            this.api = api;
            this.activator = new ViewModelActivator();

            using (GetSharedIsActivated(this, out var isActivated))
            {
                this.confirmDeleteInteraction = new Interaction<Unit, bool>();
                this.getCommand = CreateGetCommand(this, scheduler);
                this.saveCommand = CreateSaveCommand(this, scheduler);
                this.deleteCommand = CreateDeleteCommand(this, api, scheduler, isActivated);
                this.isBusy = TrueWhilstAnyCommandIsExecuting(this, this.getCommand, this.saveCommand, this.deleteCommand);
                this.validatedWeight = IntegralWeightsFrom(this);
                this.image = BitmapsFromImageDataIn(this, bitmapLoader);
                this.error = MergeErrorsIn(this, this.getCommand, this.saveCommand, this.deleteCommand);

                PopulateFromGetCommandResults(this);
                GetDataUponActivation(this.getCommand, isActivated);
                SaveDataTwoSecondsAfterItChanges(this, scheduler, isActivated, this.saveCommand);
            }
        }

        public ViewModelActivator Activator => this.activator;

        public ReactiveCommand<Unit, Unit> DeleteCommand => this.deleteCommand;

        public Interaction<Unit, bool> ConfirmDeleteInteraction => this.confirmDeleteInteraction;

        public bool IsBusy => this.isBusy.Value;

        public Error Error => this.error.Value;

        public string Name
        {
            get => this.name;
            set => this.RaiseAndSetIfChanged(ref this.name, value);
        }

        public string Weight
        {
            get => this.weight;
            set => this.RaiseAndSetIfChanged(ref this.weight, value);
        }

        public Validated<int> ValidatedWeight => this.validatedWeight.Value;

        public byte[] ImageData
        {
            get => this.imageData;
            set => this.RaiseAndSetIfChanged(ref this.imageData, value);
        }

        public IBitmap Image => this.image.Value;

        private IObservable<Dinosaur> GetDinosaur() =>
            this
                .api
                .GetDinosaur(this.id);

        private IObservable<Unit> SaveDinosaur() =>
            this
                .api
                .SaveDinosaur(new Dinosaur(this.Name, this.ValidatedWeight.Value, this.ImageData));

        private IObservable<Unit> DeleteDinosaur() =>
            this
                .confirmDeleteInteraction
                .Handle(Unit.Default)
                .SelectMany(
                    confirm =>
                    {
                        if (!confirm)
                        {
                            return Observable.Empty<Unit>();
                        }

                        return this
                            .api
                            .DeleteDinosaur(this.id);
                    });

        private void PopulateFrom(Dinosaur dinosaur)
        {
            this.Name = dinosaur?.Name;
            this.Weight = dinosaur?.Weight.ToString();
            this.ImageData = dinosaur?.Image;
        }

        private static IDisposable GetSharedIsActivated(DinosaurDetailsViewModel source, out IObservable<bool> isActivated)
        {
            var connectableIsActivated = source
                .GetIsActivated()
                .Publish();
            isActivated = connectableIsActivated;

            return Disposable
                .Create(() => connectableIsActivated.Connect());
        }

        private static ReactiveCommand<Unit, Dinosaur> CreateGetCommand(DinosaurDetailsViewModel source, IScheduler scheduler) =>
            ReactiveCommand.CreateFromObservable(
                source.GetDinosaur,
                outputScheduler: scheduler);

        private static ReactiveCommand<Unit, Unit> CreateSaveCommand(DinosaurDetailsViewModel source, IScheduler scheduler) =>
            ReactiveCommand.CreateFromObservable(
                source.SaveDinosaur,
                outputScheduler: scheduler);

        private static ReactiveCommand<Unit, Unit> CreateDeleteCommand(DinosaurDetailsViewModel source, IApi api, IScheduler scheduler, IObservable<bool> isActivated)
        {
            var canDelete = isActivated
                .Select(
                    ia =>
                    {
                        if (!ia)
                        {
                            return Observable.Empty<bool>();
                        }

                        // For the purposes of this sample, we assume IsAuditingAvailable ticks on the main thread. Otherwise, we'd need an ObserveOn.
                        return api.IsAuditingAvailable;
                    })
                .Switch();
            return ReactiveCommand.CreateFromObservable(
                source.DeleteDinosaur,
                canDelete,
                outputScheduler: scheduler);
        }

        private static ObservableAsPropertyHelper<bool> TrueWhilstAnyCommandIsExecuting(DinosaurDetailsViewModel source, params ReactiveCommand[] commands) =>
            Observable
                .CombineLatest(
                    commands.Select(command => command.IsExecuting),
                    (isExecuting) => isExecuting.Any(ie => ie))
                .ToProperty(source, x => x.IsBusy);

        private static ObservableAsPropertyHelper<Validated<int>> IntegralWeightsFrom(DinosaurDetailsViewModel source) =>
            source
                .WhenAnyValue(x => x.Weight)
                .Select(
                    weight =>
                    {
                        if (int.TryParse(weight, out var validatedWeight))
                        {
                            return Validated<int>.WithValue(validatedWeight);
                        }

                        return Validated<int>.WithError($"'{weight}' is not a valid weight. Please enter whole numbers only.");
                    })
                .ToProperty(source, x => x.ValidatedWeight);

        private static ObservableAsPropertyHelper<IBitmap> BitmapsFromImageDataIn(DinosaurDetailsViewModel source, IBitmapLoader bitmapLoader) =>
            source
                .WhenAnyValue(x => x.ImageData)
                .SelectMany(
                    imageData =>
                        Observable
                            .Using(
                                () => new MemoryStream(imageData ?? Array.Empty<byte>()),
                                stream => bitmapLoader.Load(stream, null, null).ToObservable()))
                .ToProperty(source, x => x.Image);

        private static ObservableAsPropertyHelper<Error> MergeErrorsIn(DinosaurDetailsViewModel source, ReactiveCommand<Unit, Dinosaur> getCommand, ReactiveCommand<Unit, Unit> saveCommand, ReactiveCommand<Unit, Unit> deleteCommand) =>
            Observable
                .Merge(
                    getCommand
                        .ThrownExceptions
                        .Select(ex => new Error(ex, getCommand)),
                    getCommand
                        .Select(_ => (Error)null),
                    saveCommand
                        .ThrownExceptions
                        .Select(ex => new Error(ex, saveCommand)),
                    saveCommand
                        .Select(_ => (Error)null),
                    deleteCommand
                        .ThrownExceptions
                        .Select(ex => new Error(ex, deleteCommand)),
                    deleteCommand
                        .Select(_ => (Error)null))
                .ToProperty(source, x => x.Error);

        private static void PopulateFromGetCommandResults(DinosaurDetailsViewModel source) =>
            source
                .getCommand
                .Subscribe(source.PopulateFrom);

        private static void GetDataUponActivation(ReactiveCommand command, IObservable<bool> isActivated) =>
            isActivated
                .Where(ia => ia)
                .Select(_ => Unit.Default)
                .InvokeCommand(command);

        private static void SaveDataTwoSecondsAfterItChanges(DinosaurDetailsViewModel source, IScheduler scheduler, IObservable<bool> isActivated, ReactiveCommand saveCommand)
        {
            var trigger = Observable
                .Merge(
                    source
                        .WhenAnyValue(x => x.Name, x => x.ValidatedWeight, x => x.Image, IsValid)
                        .Skip(1)
                        .Where(isValid => isValid)
                        .Throttle(TimeSpan.FromSeconds(2), scheduler)
                        .Select(_ => Unit.Default),
                    isActivated
                        .Where(ia => !ia)
                        .Select(_ => Unit.Default));

            trigger
                .InvokeCommand(saveCommand);
        }

        private static bool IsValid(string name, Validated<int> validatedWeight, IBitmap image) =>
            validatedWeight.IsValid;
    }
}