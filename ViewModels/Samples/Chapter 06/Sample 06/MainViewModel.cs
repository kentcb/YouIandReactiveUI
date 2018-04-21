namespace Book.ViewModels.Samples.Chapter06.Sample06
{
    using System;
    using System.Linq.Expressions;
    using System.Reactive;
    using System.Reactive.Linq;
    using ReactiveUI;

    [Sample(
        "WhenAnyDynamic",
        @"This sample demonstrates the use of `WhenAnyDynamic` to monitor a property for changes.

WhenAnyDynamic is used to monitor the `Name` property. Upon executing, the property is changed.")]
    public sealed class MainViewModel : ConsoleSampleViewModel
    {
        private string name;

        public MainViewModel()
        {
            var param = Expression.Parameter(typeof(MainViewModel));
            var e = Expression
                .PropertyOrField(
                    param,
                    "Name");

            this
                .WhenAnyDynamic(e, x => x.Value)
                .Subscribe(x => this.WriteLine((string)x));
        }

        public string Name
        {
            get => this.name;
            set => this.RaiseAndSetIfChanged(ref this.name, value);
        }

        protected override IObservable<Unit> Execute()
        {
            this.Name = "Ichthyosaurus";
            this.Name = "Plesiosaurus";

            return Observable.Return(Unit.Default);
        }
    }
}