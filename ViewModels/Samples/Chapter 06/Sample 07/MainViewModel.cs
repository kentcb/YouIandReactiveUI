namespace Book.ViewModels.Samples.Chapter06.Sample07
{
    using System;
    using System.Linq.Expressions;
    using System.Reactive;
    using System.Reactive.Linq;
    using ReactiveUI;

    [Sample(
        "WhenAnyDynamic against multiple properties",
        @"This sample demonstrates the use of `WhenAnyDynamic` to monitor multiple properties for changes.

WhenAnyDynamic is used to monitor both the `Name` and `Address.Postcode` properties. Upon execution, both of these properties are modified.")]
    public sealed class MainViewModel : ConsoleSampleViewModel
    {
        private AddressViewModel address;
        private string name;

        public MainViewModel()
        {
            var param = Expression.Parameter(typeof(MainViewModel));
            var nameExpression = Expression
                .PropertyOrField(
                    param,
                    "Name");
            var postcodeExpression = Expression
                .PropertyOrField(
                    Expression
                        .PropertyOrField(
                            param,
                            "Address"),
                    "Postcode");

            this
                .WhenAnyDynamic(
                    nameExpression,
                    postcodeExpression,
                    (name, postcode) => name.Value + " is located in " + postcode.Value)
                .Subscribe(x => this.WriteLine(x));
        }

        public AddressViewModel Address
        {
            get => this.address;
            set => this.RaiseAndSetIfChanged(ref this.address, value);
        }

        public string Name
        {
            get => this.name;
            set => this.RaiseAndSetIfChanged(ref this.name, value);
        }

        protected override IObservable<Unit> Execute()
        {
            this.Name = "London Natural History Museum";
            this.Address = new AddressViewModel();
            this.Address.Postcode = "SW7 5BD";

            this.Name = null;
            this.Address.Postcode = null;

            return Observable.Return(Unit.Default);
        }
    }
}