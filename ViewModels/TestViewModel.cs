namespace Book.ViewModels
{
    using System;
    using System.Linq;
    using System.Reactive;
    using System.Reactive.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using ReactiveUI;

    public sealed class TestViewModel : ReactiveObject
    {
        private readonly MethodInfo method;
        private readonly object target;
        private readonly char testLetter;
        private readonly ReactiveCommand<Unit, TestResult> executeCommand;
        private readonly ObservableAsPropertyHelper<TestResult> result;
        private TestStatus status;

        public TestViewModel(
            MethodInfo method,
            object target,
            int index)
        {
            this.method = method;
            this.target = target;
            this.testLetter = (char)('A' + index);

            var canExecute = this
                .WhenAnyValue(x => x.Status)
                .Select(status => status == TestStatus.Idle);
            this.executeCommand = ReactiveCommand.CreateFromObservable(
                this.Execute,
                canExecute);

            this
                .executeCommand
                .IsExecuting
                .Select(isExecuting => isExecuting ? TestStatus.Executing : TestStatus.Idle)
                .Do(status => this.Status = status)
                .Subscribe();

            this.result = this
                .executeCommand
                .StartWith(TestResult.Unknown)
                .ToProperty(this, x => x.Result);
        }

        public ReactiveCommand<Unit, TestResult> ExecuteCommand => this.executeCommand;

        public char TestLetter => this.testLetter;

        public string Name => this.method.Name.Replace('_', ' ');

        public TestStatus Status
        {
            get => this.status;
            private set => this.RaiseAndSetIfChanged(ref this.status, value);
        }

        public TestResult Result => this.result.Value;

        private IObservable<TestResult> Execute() =>
            Observable
                .Return(Unit.Default)
                // Make sure the test executes on a taskpool thread.
                .ObserveOn(RxApp.TaskpoolScheduler)
                .SelectMany(
                    _ =>
                        Observable
                            .StartAsync(
                                async () =>
                                {
                                    try
                                    {
                                        var task = this.method.Invoke(this.target, null) as Task;

                                        if (task == null)
                                        {
                                            return TestResult.Success;
                                        }

                                        await task;
                                        return TestResult.Success;
                                    }
                                    catch
                                    {
                                        return TestResult.Failure;
                                    }
                                }));
    }
}