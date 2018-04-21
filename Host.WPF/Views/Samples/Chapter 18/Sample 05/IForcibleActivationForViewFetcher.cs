namespace Book.Views.Samples.Chapter18.Sample05
{
    using ReactiveUI;
    using System;

    public interface IForcibleActivationForViewFetcher : IActivationForViewFetcher
    {
        IDisposable ForceAll();

        IDisposable Force(IActivatable view);
    }
}