namespace Book.ViewModels.Samples.Chapter24.Sample03
{
    using System;
    using System.Reactive;
    using System.Reactive.Linq;
    using PCLMock;

    // I'm using PCLMock here (without code generation and hard-coded to loose behavior), but you can of course use whatever you fancy.
    public sealed class ApiMock : MockBase<IApi>, IApi
    {
        public ApiMock()
            : base(MockBehavior.Loose)
        {
            this.ConfigureLooseBehavior();
        }

        public IObservable<bool> IsAuditingAvailable => this.Apply(x => x.IsAuditingAvailable);

        public IObservable<Unit> DeleteDinosaur(int id) =>
            this.Apply(x => x.DeleteDinosaur(id));

        public IObservable<Dinosaur> GetDinosaur(int id) =>
            this.Apply(x => x.GetDinosaur(id));

        public IObservable<Unit> SaveDinosaur(Dinosaur dinosaur) =>
            this.Apply(x => x.SaveDinosaur(dinosaur));

        private void ConfigureLooseBehavior()
        {
            this
                .When(x => x.IsAuditingAvailable)
                .Return(Observable.Return(true));
            this
                .When(x => x.DeleteDinosaur(It.IsAny<int>()))
                .Return(Observable.Return(Unit.Default));
            this
                .When(x => x.GetDinosaur(It.IsAny<int>()))
                .Return(Observable.Return(default(Dinosaur)));
            this
                .When(x => x.SaveDinosaur(It.IsAny<Dinosaur>()))
                .Return(Observable.Return(Unit.Default));
        }
    }
}