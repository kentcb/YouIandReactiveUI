namespace Book.ViewModels.Samples.Chapter14.Sample05
{
    using System;
    using System.Linq;
    using System.Reactive.Linq;
    using ReactiveUI;

    [Sample(
        "OneWayBind: conversion hints",
        @"This sample demonstrates the use of `OneWayBind` with a conversion hint. The type converter can optionally be told how to format the `Timestamp` by passing in a conversion hint. This hint is passed into the `OneWayBind` call.")]
    public sealed class MainViewModel : ReactiveObject
    {
        private readonly ObservableAsPropertyHelper<Timestamp> time;

        public MainViewModel()
        {
            this.time = Observable
                .Timer(TimeSpan.Zero, TimeSpan.FromSeconds(1), RxApp.MainThreadScheduler)
                .Select(_ => new Timestamp(DateTime.Now.Ticks))
                .ToProperty(this, x => x.Time);
        }

        public Timestamp Time => this.time.Value;
    }
}