namespace Book.ViewModels.Samples.Chapter05.Sample04
{
    using System;
    using System.Linq;
    using System.Reactive;
    using System.Reactive.Linq;

    [Sample(
        "Delaying Change Notifications",
        @"This sample demonstrates the use of the `DelayChangeNotifications` method provided by `ReactiveObject`.")]
    public sealed class MainViewModel : ConsoleSampleViewModel
    {
        protected override IObservable<Unit> Execute()
        {
            var dinosaur = new DinosaurViewModel();
            dinosaur
                .Changed
                .Select(x => $"{x.PropertyName} changed.")
                .Subscribe(this.WriteLine);

            using (dinosaur.DelayChangeNotifications())
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

            return Observable.Return(Unit.Default);
        }
    }
}