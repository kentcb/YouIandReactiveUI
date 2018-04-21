namespace Book.ViewModels.Samples.Chapter14.Sample11
{
    using System;
    using System.Reactive.Linq;
    using ReactiveUI;

    [Sample(
        "Bind: signal view update",
        @"This sample demonstrates how a custom signal can be passed to `Bind` to indicate when values should propagate from the view back to the view model. In this case, changes made by the user are throttled for a full second, ensuring that the view model receives at most one change to the `Name` property per second.")]
    public sealed class MainViewModel : ReactiveObject
    {
        private readonly ObservableAsPropertyHelper<string> display;
        private string name;

        public MainViewModel()
        {
            this.display = this
                .WhenAnyValue(x => x.Name)
                .Select(name => this.DetermineSalary(name))
                .Switch()
                .Select(salary => salary == null ? "Enter your name to determine your salary as a paleontologist." : $"As a paleontologist, you would earn ${salary}K per annum.")
                .ToProperty(this, x => x.Display);
        }

        public string Name
        {
            get => this.name;
            set => this.RaiseAndSetIfChanged(ref this.name, value);
        }

        public string Display => this.display.Value;

        private IObservable<decimal?> DetermineSalary(string name)
        {
            if (name == null)
            {
                return Observable.Return<decimal?>(null);
            }

            return Observable
                .Return((decimal?)(name.Length * 100m))
                .Delay(TimeSpan.FromSeconds(1), RxApp.MainThreadScheduler);
        }
    }
}