namespace Book.ViewModels.Samples.Chapter06.Sample01
{
    using System;
    using System.Linq;
    using System.Reactive.Linq;
    using ReactiveUI;

    [Sample(
        "WhenAny",
        @"This sample demonstrates the use of `WhenAny` to monitor a property for changes.

Whenever the `Name` property changes, a `Message` property is updated with a value that is derived from the current value of the `Name` property.")]
    public sealed class MainViewModel : ReactiveObject
    {
        private string name;
        private string message;

        public MainViewModel()
        {
            // poor man's WhenAny
            //this
            //    .Changed
            //    .Where(change => change.PropertyName == nameof(Name))
            //    .Select(_ => this.Name)
            //    .Select(this.GetMessageFor)
            //    .Do(message => this.Message = message)
            //    .Subscribe();

            this
                .WhenAny(x => x.Name, x => x.Value)
                .Select(this.GetMessageFor)
                .Do(message => this.Message = message)
                .Subscribe();
        }

        public string Name
        {
            get => this.name;
            set => this.RaiseAndSetIfChanged(ref this.name, value);
        }

        public string Message
        {
            get => this.message;
            private set => this.RaiseAndSetIfChanged(ref this.message, value);
        }

        private string GetMessageFor(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return "Enter your name above.";
            }

            return $"Dear {name}, you are cordially invited to my Dinosaur birthday party. Bring your ROAR.";
        }
    }
}