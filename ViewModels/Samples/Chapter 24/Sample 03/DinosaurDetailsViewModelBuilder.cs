namespace Book.ViewModels.Samples.Chapter24.Sample03
{
    using System.Reactive.Concurrency;
    using Genesis.TestUtil;
    using Microsoft.Reactive.Testing;
    using Splat;

    public sealed class DinosaurDetailsViewModelBuilder : IBuilder
    {
        private bool activate;
        private int id;
        private IBitmapLoader bitmapLoader;
        private IApi api;
        private IScheduler mainScheduler;
        private IScheduler timerScheduler;
        private bool? confirmDeleteInteraction;

        public DinosaurDetailsViewModelBuilder()
        {
            this.activate = true;
            this.id = 42;
            this.bitmapLoader = new BitmapLoaderMockBuilder().Build();
            this.api = new ApiMockBuilder().Build();
            this.mainScheduler = CurrentThreadScheduler.Instance;
            this.timerScheduler = new SchedulerMock();
        }

        public DinosaurDetailsViewModelBuilder WithActivation(bool activate = true) =>
            this.With(ref this.activate, activate);

        public DinosaurDetailsViewModelBuilder WithId(int id) =>
            this.With(ref this.id, id);

        public DinosaurDetailsViewModelBuilder WithBitmapLoader(IBitmapLoader bitmapLoader) =>
            this.With(ref this.bitmapLoader, bitmapLoader);

        public DinosaurDetailsViewModelBuilder WithApi(IApi api) =>
            this.With(ref this.api, api);

        public DinosaurDetailsViewModelBuilder WithApiMock(out ApiMock apiMock) =>
            this
                .WithApi(apiMock = new ApiMock());

        public DinosaurDetailsViewModelBuilder WithMainScheduler(IScheduler scheduler) =>
            this.With(ref this.mainScheduler, scheduler);

        public DinosaurDetailsViewModelBuilder WithTimerScheduler(IScheduler timerScheduler) =>
            this.With(ref this.timerScheduler, timerScheduler);

        public DinosaurDetailsViewModelBuilder WithTestScheduler(out TestScheduler testScheduler) =>
            this
                .WithMainScheduler(testScheduler = new TestScheduler())
                .WithTimerScheduler(testScheduler);

        public DinosaurDetailsViewModelBuilder WithDinosaur(Dinosaur dinosaur) =>
            this.WithApi(
                new ApiMockBuilder()
                    .WithGetDinosaur(dinosaur)
                    .Build());

        public DinosaurDetailsViewModelBuilder WithConfirmDeleteInteraction(bool confirmDeleteInteraction) =>
            this.With(ref this.confirmDeleteInteraction, confirmDeleteInteraction);

        public DinosaurDetailsViewModel Build()
        {
            var result = new DinosaurDetailsViewModel(
                this.id,
                this.bitmapLoader,
                this.api,
                this.mainScheduler,
                this.timerScheduler);

            // Make the weight valid by default.
            result.Weight = "42";

            if (this.activate)
            {
                result
                    .Activator
                    .Activate();
            }

            if (this.confirmDeleteInteraction.HasValue)
            {
                result
                    .ConfirmDeleteInteraction
                    .RegisterHandler(context => context.SetOutput(this.confirmDeleteInteraction.Value));
            }

            return result;
        }

        public static implicit operator DinosaurDetailsViewModel(DinosaurDetailsViewModelBuilder builder) =>
            builder.Build();
    }
}
