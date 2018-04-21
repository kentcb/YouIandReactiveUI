namespace Book.ViewModels.Samples.Chapter06.Sample03
{
    using System;
    using System.Linq;
    using System.Reactive.Linq;
    using ReactiveUI;

    [Sample(
        "WhenAnyValue against multiple properties",
        @"This sample demonstrates the use of `WhenAnyValue` to monitor multiple properties for changes.

Whenever the `FirstName` or `LastName` properties changes, a `Message` property is updated with a value that is derived from the current values of those properties.")]
    public sealed class MainViewModel : ReactiveObject
    {
        private string firstName;
        private string lastName;
        private string message;

        public MainViewModel()
        {
            this
                .WhenAnyValue(x => x.FirstName, x => x.LastName, this.GetMessageFor)
                .Do(message => this.Message = message)
                .Subscribe();
        }

        public string FirstName
        {
            get => this.firstName;
            set => this.RaiseAndSetIfChanged(ref this.firstName, value);
        }

        public string LastName
        {
            get => this.lastName;
            set => this.RaiseAndSetIfChanged(ref this.lastName, value);
        }

        public string Message
        {
            get => this.message;
            private set => this.RaiseAndSetIfChanged(ref this.message, value);
        }

        private string GetMessageFor(string firstName, string lastName)
        {
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                return "Enter your name above.";
            }

            return $"Dear {firstName} {lastName}, you are cordially invited to my Dinosaur birthday party. Bring your ROAR.";
        }
    }
}