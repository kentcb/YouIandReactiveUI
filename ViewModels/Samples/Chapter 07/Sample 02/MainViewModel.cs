namespace Book.ViewModels.Samples.Chapter07.Sample02
{
    using System;
    using System.Linq;
    using System.Reactive.Linq;
    using ReactiveUI;

    [Sample(
        "ToProperty with scheduling",
        @"This sample demonstrates the use of `ToProperty` to create a property that is based on an observable that ticks off the main thread.

The `Seconds` property in the `MainViewModel` is based on an observable (a timer) that ticks every second on a non-UI thread. As such, we need to pass `MainThreadScheduler` to `ToProperty` to ensure changes occur on the correct thread.")]
    public sealed class MainViewModel : ReactiveObject
    {
        private readonly ObservableAsPropertyHelper<int> seconds;

        public MainViewModel()
        {
            // Here is a contrived observable that ticks off the main thread. Of course, in reality we could just pass MainThreadScheduler into our Timer call
            // to ensure it continues execution on the UI thread. If we did that, we could remove the scheduler passed into ToProperty and gain some performance
            // by using the CurrentThreadScheduler instead.
            var timer = Observable
                .Timer(TimeSpan.Zero, TimeSpan.FromSeconds(1), RxApp.TaskpoolScheduler);

            // Because timer ticks off the main thread, we need to tell ToProperty to use MainThreadScheduler rather than the default CurrentThreadScheduler. If
            // you try removing the scheduler, you'll see that the UI crashes because the property is being modified off the UI thread.
            this.seconds = timer
                .Scan(0, (acc, _) => ++acc)
                .ToProperty(this, x => x.Seconds, scheduler: RxApp.MainThreadScheduler);
        }

        public int Seconds => this.seconds.Value;
    }
}