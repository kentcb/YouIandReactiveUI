namespace Book.Views.Samples.Chapter18.Sample05
{
    using System;
    using System.Collections.Generic;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;
    using System.Reflection;
    using System.Windows;
    using ReactiveUI;

    public sealed class ForcibleActivationForViewFetcher : IForcibleActivationForViewFetcher
    {
        private readonly IActivationForViewFetcher deferTo;
        private readonly BehaviorSubject<bool> globalForce;
        private readonly IDictionary<IActivatable, BehaviorSubject<bool>> forcedViews;

        public ForcibleActivationForViewFetcher(IActivationForViewFetcher deferTo)
        {
            this.deferTo = deferTo;
            this.globalForce = new BehaviorSubject<bool>(false);
            this.forcedViews = new Dictionary<IActivatable, BehaviorSubject<bool>>();
        }

        public IDisposable ForceAll()
        {
            this.globalForce.OnNext(true);
            return Disposable.Create(() => this.globalForce.OnNext(false));
        }

        public IDisposable Force(IActivatable view)
        {
            this.EnsureSubjectFor(view).OnNext(true);
            return Disposable.Create(() => this.EnsureSubjectFor(view).OnNext(false));
        }

        public int GetAffinityForView(Type view) =>
            (typeof(FrameworkElement).GetTypeInfo().IsAssignableFrom(view.GetTypeInfo())) ? 100 : 0;

        public IObservable<bool> GetActivationForView(IActivatable view) =>
            Observable
                .CombineLatest(
                    this.globalForce,
                    this.EnsureSubjectFor(view),
                    this.deferTo.GetActivationForView(view).StartWith(false),
                    (forcedGlobally, forcedLocally, activated) => forcedGlobally || forcedLocally || activated)
                .DistinctUntilChanged();

        private BehaviorSubject<bool> EnsureSubjectFor(IActivatable view)
        {
            BehaviorSubject<bool> subject;

            if (!this.forcedViews.TryGetValue(view, out subject))
            {
                this.forcedViews[view] = subject = new BehaviorSubject<bool>(false);
            }

            return subject;
        }
    }
}