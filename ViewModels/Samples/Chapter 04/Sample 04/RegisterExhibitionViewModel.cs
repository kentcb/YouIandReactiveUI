namespace Book.ViewModels.Samples.Chapter04.Sample04
{
    using System;
    using System.Linq;
    using System.Reactive;
    using System.Reactive.Linq;
    using ReactiveUI;
    using Splat;

    public sealed class RegisterExhibitionViewModel : ReactiveObject
    {
        private readonly ReactiveCommand<Unit, Unit> registerCommand;
        private readonly ObservableAsPropertyHelper<DateTimeOffset?> when;
        private string what;
        private string whenEntry;
        private string where;

        public RegisterExhibitionViewModel()
        {
            this.when = this
                .WhenAnyValue(x => x.WhenEntry)
                .Select(ParseDateTimeOffset)
                .ToProperty(this, x => x.When);
            var canRegister = this
                .WhenAnyValue(
                    x => x.What,
                    x => x.When,
                    x => x.Where,
                    (what, when, where) => !string.IsNullOrEmpty(what) && when != null && !string.IsNullOrEmpty(where));
            this.registerCommand = ReactiveCommand
                .Create(
                    () =>
                    {
                        var exhibition = new DinosaurExhibition(this.What, this.When.Value, this.Where);
                        Locator
                            .CurrentMutable
                            .RegisterConstant<IDinosaurExhibition>(exhibition);

                        this.What = null;
                        this.WhenEntry = null;
                        this.Where = null;
                    },
                    canRegister,
                    outputScheduler: RxApp.MainThreadScheduler);
        }

        public ReactiveCommand<Unit, Unit> RegisterCommand => this.registerCommand;

        public string What
        {
            get => this.what;
            set => this.RaiseAndSetIfChanged(ref this.what, value);
        }

        public string WhenEntry
        {
            get => this.whenEntry;
            set => this.RaiseAndSetIfChanged(ref this.whenEntry, value);
        }

        public string Where
        {
            get => this.where;
            set => this.RaiseAndSetIfChanged(ref this.where, value);
        }

        public DateTimeOffset? When => this.when.Value;

        private static DateTimeOffset? ParseDateTimeOffset(string input)
        {
            DateTimeOffset dateTimeOffset;

            if (!DateTimeOffset.TryParse(input, out dateTimeOffset))
            {
                return null;
            }

            return dateTimeOffset;
        }

        private sealed class DinosaurExhibition : IDinosaurExhibition
        {
            private readonly string what;
            private readonly DateTimeOffset when;
            private readonly string where;

            public DinosaurExhibition(
                string what,
                DateTimeOffset when,
                string where)
            {
                this.what = what;
                this.when = when;
                this.where = where;
            }

            public string What => this.what;

            public DateTimeOffset When => this.when;

            public string Where => this.where;
        }
    }
}