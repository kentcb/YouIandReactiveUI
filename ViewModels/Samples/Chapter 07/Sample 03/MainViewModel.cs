namespace Book.ViewModels.Samples.Chapter07.Sample03
{
    using System;
    using System.Linq;
    using System.Reactive.Linq;
    using ReactiveUI;

    [Sample(
        "ToProperty with initial value",
        @"This sample demonstrates the use of `ToProperty` against a non-behavioral observable, using the `initialValue` parameter to seed the property's value.

The `IsConnected` property in `MainViewModel` is based on an observable that doesn't tick an initial value for 3 seconds. During that period, the property would ordinarily have a value of `default(bool)`, which is `false`. By passing in `true` to the `initialValue` parameter of `ToProperty`, we can override that default. Hence, the message below defaults to saying there is a connection and then only changes once the underlying observable starts ticking values.")]
    public sealed class MainViewModel : ReactiveObject
    {
        private readonly ObservableAsPropertyHelper<bool> isConnected;
        private readonly ObservableAsPropertyHelper<string> message;

        public MainViewModel()
        {
            // Here is a contrived observable that takes 3 seconds to tick an initial value. Thereafter, it will bounce between true and false
            // every few seconds.
            var isConnected = Observable
                .Timer(TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(3), RxApp.MainThreadScheduler)
                .Scan(true, (acc, _) => !acc);

            // Create an IsConnected property based on the above observable. If we don't specify an initial value, IsConnected will default to
            // default(bool), which is false. By passing in true we tell ToProperty that it should default the property to true until we receive
            // a value from the observable pipeline.
            this.isConnected = isConnected
                .ToProperty(this, x => x.IsConnected, initialValue: true);

            // Just another property that transforms IsConnected into an appropriate message for display.
            this.message = this
                .WhenAnyValue(x => x.IsConnected)
                .Select(ic => ic ? "You are connected to the Internet. Retrieving dinosaur exhibitions..." : "No Internet connection. Exhibitions cannot be retrieved.")
                .ToProperty(this, x => x.Message);
        }

        public bool IsConnected => this.isConnected.Value;

        public string Message => this.message.Value;
    }
}