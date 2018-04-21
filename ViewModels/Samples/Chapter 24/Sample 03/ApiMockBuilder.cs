namespace Book.ViewModels.Samples.Chapter24.Sample03
{
    using System;
    using System.Reactive;
    using System.Reactive.Linq;
    using Genesis.TestUtil;
    using PCLMock;

    public sealed class ApiMockBuilder : IBuilder
    {
        private readonly ApiMock api;

        public ApiMockBuilder()
        {
            this.api = new ApiMock();
        }

        public ApiMockBuilder WithIsAuditingAvailable(IObservable<bool> isAuditingAvailable)
        {
            this
                .api
                .When(x => x.IsAuditingAvailable)
                .Return(isAuditingAvailable);
            return this;
        }

        public ApiMockBuilder WithIsAuditingAvailable(bool isAuditingAvailable) =>
            this
                .WithIsAuditingAvailable(Observable.Return(isAuditingAvailable));

        public ApiMockBuilder WithGetDinosaur(IObservable<Dinosaur> getDinosaur)
        {
            this
                .api
                .When(x => x.GetDinosaur(It.IsAny<int>()))
                .Return(getDinosaur);
            return this;
        }

        public ApiMockBuilder WithGetDinosaur(Dinosaur getDinosaur) =>
            this
                .WithGetDinosaur(Observable.Return(getDinosaur));

        public ApiMockBuilder WithGetDinosaur(Exception getDinosaur) =>
            this
                .WithGetDinosaur(Observable.Throw<Dinosaur>(getDinosaur));

        public ApiMockBuilder WithSaveDinosaur(IObservable<Unit> saveDinosaur)
        {
            this
                .api
                .When(x => x.SaveDinosaur(It.IsAny<Dinosaur>()))
                .Return(saveDinosaur);
            return this;
        }

        public ApiMockBuilder WithSaveDinosaur(Exception saveDinosaur) =>
            this
                .WithSaveDinosaur(Observable.Throw<Unit>(saveDinosaur));

        public ApiMockBuilder WithDeleteDinosaur(IObservable<Unit> deleteDinosaur)
        {
            this
                .api
                .When(x => x.DeleteDinosaur(It.IsAny<int>()))
                .Return(deleteDinosaur);
            return this;
        }

        public ApiMockBuilder WithDeleteDinosaur(Exception deleteDinosaur) =>
            this
                .WithDeleteDinosaur(Observable.Throw<Unit>(deleteDinosaur));

        public ApiMock Build() =>
            this.api;

        public static implicit operator ApiMock(ApiMockBuilder builder) =>
            builder.Build();
    }
}