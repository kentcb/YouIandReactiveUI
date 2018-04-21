namespace Book.ViewModels.Samples.Chapter06.Sample08
{
    using System;
    using System.Linq;
    using System.Reactive.Linq;
    using ReactiveUI;

    [Sample(
        "WhenAny* behavioral semantics",
        @"This sample demonstrates the behavioral semantics of `WhenAnyValue` (and the `WhenAny*` operators in general).

Even though `WhenAnyValue` is called after the `age` is set to `4`, it receives the existing value rather than ""missing"" it.")]
    public sealed class MainViewModel : ReactiveObject
    {
        private int? age;
        private string message;

        public MainViewModel()
        {
            this.age = 4;

            this
                .WhenAnyValue(x => x.Age, this.GetMessageFor)
                //.Skip(1)
                .Do(message => this.Message = message)
                .Subscribe();
        }

        public int? Age
        {
            get => this.age;
            set => this.RaiseAndSetIfChanged(ref this.age, value);
        }

        public string Message
        {
            get => this.message;
            private set => this.RaiseAndSetIfChanged(ref this.message, value);
        }

        private string GetMessageFor(int? age)
        {
            if (age == null)
            {
                return null;
            }

            if (age < 3)
            {
                return "Definitely too young for this exhibition.";
            }
            if (age < 5)
            {
                return "This exhibition is a bit scary - keep your child close.";
            }

            return "Your child should be fine with this exhibition.";
        }
    }
}