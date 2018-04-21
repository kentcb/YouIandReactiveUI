namespace Book.ViewModels.Samples.Chapter23.Sample01
{
    using System;
    using System.Reactive;
    using System.Reactive.Linq;
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Attributes.Jobs;
    using BenchmarkDotNet.Engines;
    using ReactiveUI;

    [MemoryDiagnoser]
    [SimpleJob(
        RunStrategy.Monitoring,
        launchCount: 2,
        warmupCount: 1,
        targetCount: 2,
        invocationCount: 100000)]
    public class Tests
    {
        private readonly RepresentativeReactiveObject representativeReactiveObject;
        private readonly IObservable<int> neverIntObservable;

        public Tests()
        {
            this.representativeReactiveObject = new RepresentativeReactiveObject();
            this.neverIntObservable = Observable.Never<int>();
        }

        [Benchmark(Baseline = true)]
        public object create_object() =>
            new object();

        [Benchmark]
        public object create_reactive_object() =>
            new ConcreteReactiveObject();

        [Benchmark]
        public object create_pipeline_using_when_any_value() =>
            this
                .representativeReactiveObject
                .WhenAnyValue(x => x.ThrownExceptions);

        [Benchmark]
        public object create_observable_as_property_helper() =>
            new ObservableAsPropertyHelper<int>(this.neverIntObservable, _ => { }, onChanging: null);

        [Benchmark]
        public object create_reactive_command() =>
            ReactiveCommand.CreateFromObservable<Unit, int>(_ => this.neverIntObservable);

        [Benchmark]
        public object create_interaction() =>
            new Interaction<Unit, Unit>();

        [Benchmark]
        public object create_reactive_list() =>
            new ReactiveList<int>();

        private sealed class ConcreteReactiveObject : ReactiveObject
        {
        }

        private sealed class RepresentativeReactiveObject : ReactiveObject
        {

        }
    }
}