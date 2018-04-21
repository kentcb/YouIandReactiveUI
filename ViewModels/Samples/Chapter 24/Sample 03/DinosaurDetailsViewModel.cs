namespace Book.ViewModels.Samples.Chapter24.Sample03
{
    using System;
    using System.IO;
    using System.Reactive;
    using System.Reactive.Concurrency;
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
            IScheduler mainScheduler,
            IScheduler timerScheduler)
        {
            this.id = id;
            this.api = api;
            this.activator = new ViewModelActivator();

            var publishedIsActivated = this
                .GetIsActivated()
                .Publish();

            this.getCommand = ReactiveCommand.CreateFromObservable(
                this.GetDinosaur,
                outputScheduler: mainScheduler);

            this.saveCommand = ReactiveCommand.CreateFromObservable(
                this.SaveDinosaur,
                outputScheduler: mainScheduler);

            this.confirmDeleteInteraction = new Interaction<Unit, bool>();

            var canDelete = publishedIsActivated
                .Select(
                    isActivated =>
                    {
                        if (!isActivated)
                        {
                            return Observable.Empty<bool>();
                        }

                        // For the purposes of this sample, we assume IsAuditingAvailable ticks on the main thread. Otherwise, we'd need an ObserveOn.
                        return this.api.IsAuditingAvailable;
                    })
                .Switch();
            this.deleteCommand = ReactiveCommand.CreateFromObservable(
                this.DeleteDinosaur,
                canDelete,
                outputScheduler: mainScheduler);

            this.isBusy = Observable
                .CombineLatest(
                    this.getCommand.IsExecuting,
                    this.saveCommand.IsExecuting,
                    this.deleteCommand.IsExecuting,
                    (isGetting, isSaving, isDeleting) => isGetting || isSaving || isDeleting)
                .ToProperty(this, x => x.IsBusy);

            this.validatedWeight = this
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
                .ToProperty(this, x => x.ValidatedWeight);

            this.image = this
                .WhenAnyValue(x => x.ImageData)
                .SelectMany(
                    imageData =>
                        Observable
                            .Using(
                                () => new MemoryStream(imageData ?? Array.Empty<byte>()),
                                stream => bitmapLoader.Load(stream, null, null).ToObservable()))
                .ToProperty(this, x => x.Image);

            this
                .getCommand
                .Subscribe(this.PopulateFrom);

            publishedIsActivated
                .Where(isActivated => isActivated)
                .Select(_ => Unit.Default)
                .InvokeCommand(this.getCommand);

            var shouldSave = Observable
                .Merge(
                    this
                        .WhenAnyValue(x => x.Name, x => x.ValidatedWeight, x => x.Image, IsValid)
                        .Skip(1)
                        .Where(isValid => isValid)
                        .Throttle(TimeSpan.FromSeconds(2), timerScheduler)
                        .Select(_ => Unit.Default),
                    publishedIsActivated
                        .Where(isActivated => !isActivated)
                        .Select(_ => Unit.Default));

            shouldSave
                .InvokeCommand(this.saveCommand);

            publishedIsActivated
                .Connect();

            this.error = Observable
                .Merge(
                    this
                        .getCommand
                        .ThrownExceptions
                        .Select(ex => new Error(ex, this.getCommand)),
                    this
                        .getCommand
                        .Select(_ => (Error)null),
                    this
                        .saveCommand
                        .ThrownExceptions
                        .Select(ex => new Error(ex, this.saveCommand)),
                    this
                        .saveCommand
                        .Select(_ => (Error)null),
                    this
                        .deleteCommand
                        .ThrownExceptions
                        .Select(ex => new Error(ex, this.deleteCommand)),
                    this
                        .deleteCommand
                        .Select(_ => (Error)null))
                .ToProperty(this, x => x.Error);
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

        private static bool IsValid(string name, Validated<int> validatedWeight, IBitmap image) =>
            validatedWeight.IsValid;
    }
}