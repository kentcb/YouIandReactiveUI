namespace Book.ViewModels.Samples.Chapter05.Sample03
{
    using System;
    using System.Linq;
    using System.Reactive;
    using System.Reactive.Linq;

    [Sample(
        "Suppressing Change Notifications",
        @"This sample demonstrates the use of the `SuppressChangeNotifications` method provided by `ReactiveObject`.")]
    public sealed class MainViewModel : ConsoleSampleViewModel
    {
        protected override IObservable<Unit> Execute()
        {
            var dinosaur = new DinosaurViewModel();
            dinosaur
                .Changed
                .Select(x => $"{x.PropertyName} changed.")
                .Subscribe(this.WriteLine);

            using (dinosaur.SuppressChangeNotifications())
            {
                dinosaur.Name = "Ophyhalmosaurus";
                dinosaur.Era = Data.Era.Mesozoic;
                dinosaur.Period = Data.Period.Jurassic;

                dinosaur.Name = "Eryops";
                dinosaur.Era = Data.Era.Palaeozoic;
                dinosaur.Period = Data.Period.Permian;

                dinosaur.Name = "Procompsognathus";
                dinosaur.Era = Data.Era.Mesozoic;
                dinosaur.Period = Data.Period.Triassic;
            }

            dinosaur.Name = "Ichthyosaurus";

            return Observable.Return(Unit.Default);
        }
    }
}