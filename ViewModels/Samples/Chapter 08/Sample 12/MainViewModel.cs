namespace Book.ViewModels.Samples.Chapter08.Sample12
{
    using System;
    using System.Reactive;
    using System.Reactive.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using ReactiveUI;

    [Sample(
        "Canceling Commands (TPL)",
        @"This sample demonstrates how reactive commands that use a `Task` for their execution logic can be canceled.

The view model exposes a `LoginCommand` and a `CancelCommand`, which can be used to cancel an in-flight login.")]
    public sealed class MainViewModel : ReactiveObject
    {
        private readonly ReactiveCommand<Unit, bool?> loginCommand;
        private readonly ReactiveCommand<Unit, Unit> cancelCommand;
        private string user;
        private string password;

        public MainViewModel()
        {
            var canLogin = this
                .WhenAnyValue(x => x.User, x => x.Password, (user, password) => !string.IsNullOrWhiteSpace(user) && !string.IsNullOrWhiteSpace(password));
            this.loginCommand = ReactiveCommand.CreateFromObservable(
                () =>
                    Observable
                        .StartAsync(this.LoginAsync)
                        .TakeUntil(this.cancelCommand),
                canLogin);

            this.cancelCommand = ReactiveCommand.Create(() => { }, this.loginCommand.IsExecuting);
        }

        public ReactiveCommand<Unit, bool?> LoginCommand => this.loginCommand;

        public ReactiveCommand<Unit, Unit> CancelCommand => this.cancelCommand;

        public string User
        {
            get => this.user;
            set => this.RaiseAndSetIfChanged(ref this.user, value);
        }

        public string Password
        {
            get => this.password;
            set => this.RaiseAndSetIfChanged(ref this.password, value);
        }

        private async Task<bool?> LoginAsync(CancellationToken ct)
        {
            var result = this.Password == "Mr. Goodbytes";
            await Task.Delay(TimeSpan.FromSeconds(1.5), ct);

            return result;
        }
    }
}