namespace Book.ViewModels.Samples.Chapter18.Sample03
{
    using System;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using ReactiveUI;

    public sealed class ChildViewModel : ReactiveObject, ISupportsActivation
    {
        private readonly ViewModelActivator activator;
        private readonly bool useExpensiveResource;

        public ChildViewModel(bool useExpensiveResource)
        {
            this.activator = new ViewModelActivator();
            this.useExpensiveResource = useExpensiveResource;

            if (this.useExpensiveResource)
            {
                this
                    .WhenActivated(
                        disposables =>
                        {
                            Observable
                                .Timer(TimeSpan.Zero, TimeSpan.FromMilliseconds(1))
                                .Select(this.ExpensiveCalculation)
                                .Subscribe()
                                .DisposeWith(disposables);
                        });
            }
        }

        public ViewModelActivator Activator => this.activator;

        public bool UseExpensiveResource => this.useExpensiveResource;

        private double ExpensiveCalculation(long l)
        {
            var sum = 0d;

            for (var i = 0; i < 100000; ++i)
            {
                sum += Math.Sin(l + i);
                sum -= Math.Cos(l - i);
                sum *= Math.Tan(l + i);
                sum /= Math.Min(1, Math.Sqrt(l - i));
            }

            return sum;
        }
    }
}