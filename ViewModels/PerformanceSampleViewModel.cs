namespace Book.ViewModels
{
    using System;
    using System.Reactive;
    using System.Reactive.Linq;
    using BenchmarkDotNet.Running;
    using ReactiveUI;

    public abstract class PerformanceSampleViewModel<TTests> : ConsoleSampleViewModel
    {
        protected override IObservable<Unit> Execute() =>
            Observable
                .Return(Unit.Default)
                // Make sure the tests execute on a taskpool thread.
                .ObserveOn(RxApp.TaskpoolScheduler)
                .Select(_ => BenchmarkRunner.Run<TTests>())
                .Do(Console.WriteLine)
                .Select(_ => Unit.Default);
    }
}