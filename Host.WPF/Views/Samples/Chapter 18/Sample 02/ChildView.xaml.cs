namespace Book.Views.Samples.Chapter18.Sample02
{
    using System;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using ReactiveUI;
    using ViewModels.Samples.Chapter18.Sample02;

    public partial class ChildView : ReactiveUserControl<ChildViewModel>
    {
        public ChildView()
        {
            InitializeComponent();

            this
                .WhenActivated(
                    disposables =>
                    {
                        this
                            .OneWayBind(this.ViewModel, x => x.UseExpensiveResource, x => x.nameLabel.Content, x => x ? "This dinosaur is using an expensive resource. Take a look at your CPU usage if you don't believe me!" : "This dinosaur is NOT using an expensive resource.")
                            .DisposeWith(disposables);

                        this
                            .WhenAnyValue(x => x.ViewModel.UseExpensiveResource)
                            .Where(x => x)
                            .Select(_ => Observable.Timer(TimeSpan.Zero, TimeSpan.FromMilliseconds(1)))
                            .Switch()
                            .Select(this.ExpensiveCalculation)
                            .Subscribe()
                            .DisposeWith(disposables);
                    });
        }

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