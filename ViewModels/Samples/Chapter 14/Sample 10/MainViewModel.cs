namespace Book.ViewModels.Samples.Chapter14.Sample10
{
    using System.Linq;
    using System.Reactive.Linq;
    using ReactiveUI;

    [Sample(
        "Bind: conversion hints",
        @"This sample demonstrates the use of `Bind` with a conversion hint. The type converters used can optionally be told how to format the `Timestamp` by passing in a conversion hint. This hint is passed into the `Bind` call.")]
    public sealed class MainViewModel : ReactiveObject
    {
        private readonly ObservableAsPropertyHelper<string> display;
        private Timestamp time;

        public MainViewModel()
        {
            this.display = this
                .WhenAnyValue(x => x.Time)
                .Select(time => $"You have entered a time with {time.Ticks} ticks.")
                .ToProperty(this, x => x.Display);
        }

        public Timestamp Time
        {
            get => this.time;
            set => this.RaiseAndSetIfChanged(ref this.time, value);
        }

        public string Display => this.display.Value;
    }
}