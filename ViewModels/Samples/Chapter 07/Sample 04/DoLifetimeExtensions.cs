namespace System.Reactive.Linq
{
    using System;
    using Disposables;

    public static class DoLifetimeExtensions
    {
        public static IObservable<T> DoLifetime<T>(this IObservable<T> @this, Action subscribed, Action unsubscribed)
        {
            return Observable
                .Create<T>(
                    observer =>
                    {
                        subscribed();

                        var disposables = new CompositeDisposable();
                        @this
                            .Subscribe(observer)
                            .DisposeWith(disposables);
                        Disposable
                            .Create(() => unsubscribed())
                            .DisposeWith(disposables);

                        return disposables;
                    });
        }
    }
}