namespace Book.ViewModels.Samples.Chapter24.Sample02
{
    using System;
    using System.Reactive;

    public interface IApi
    {
        IObservable<bool> IsAuditingAvailable
        {
            get;
        }

        IObservable<Dinosaur> GetDinosaur(int id);

        IObservable<Unit> SaveDinosaur(Dinosaur dinosaur);

        IObservable<Unit> DeleteDinosaur(int id);
    }
}