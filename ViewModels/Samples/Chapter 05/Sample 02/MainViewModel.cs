namespace Book.ViewModels.Samples.Chapter05.Sample02
{
    using System;
    using System.Reactive;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;

    [Sample(
        "Change Notifications",
        @"This simple sample demonstrates the `Changing` and `Changed` observables provided by `ReactiveObject`.")]
    public sealed class MainViewModel : ConsoleSampleViewModel
    {
        protected override IObservable<Unit> Execute() =>
            Observable
                .Create<Unit>(
                    observer =>
                    {
                        var disposables = new CompositeDisposable();
                        var dinosaur = new DinosaurViewModel();
                        dinosaur
                            .Changing
                            .Do(e => this.WriteLine("Property '{0}' changing.", e.PropertyName))
                            .Subscribe()
                            .DisposeWith(disposables);
                        dinosaur
                            .Changed
                            .Do(e => this.WriteLine("Property '{0}' changed.", e.PropertyName))
                            .Subscribe()
                            .DisposeWith(disposables);

                        dinosaur.Name = "Edaphosaurus";
                        dinosaur.Weight = 290;
                        dinosaur.Weight = 300;
                        dinosaur.Weight = 300;

                        observer.OnCompleted();

                        return disposables;
                    });
    }
}